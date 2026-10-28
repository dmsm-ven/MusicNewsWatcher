using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.API.Models;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Core.Models.Dtos;
using System.Diagnostics;

namespace MusicNewsWatcher.API.Services;

public class MusicNewsCrawler(IDbContextFactory<MusicWatcherDbContext> dbContextFactory,
        CrawlerHttpClientProviderFactory httpClientProviderFactory,
        IOptionsMonitor<CrawlerConfiguration> crawlerOptions,
        ILogger<MusicNewsCrawler> logger)
{
    public async Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders, CancellationToken stoppingToken)
    {
        logger.LogInformation("Запуск поиска обновлений всех провайдеров");

        stoppingToken.ThrowIfCancellationRequested();

        var providers = musicProviders?.ToList() ?? new List<MusicProviderBase>();

        if (providers.Count == 0)
        {
            logger.LogWarning("Нет доступных провайдеров. Отмена поиска новинок");
            return Enumerable.Empty<NewAlbumFoundResult>().ToList();
        }

        logger.LogTrace("Запуск парсера по поиску новинок для всех провайдеров ({providerNames})",
            string.Join(", ", providers.Select(m => m.Name)));

        // Для обхода нужны только артисты (без альбомов): альбомы каждого артиста читаются позже в RefreshArtist
        var providerToArtists = new Dictionary<MusicProviderBase, List<ArtistEntity>>();

        await using (var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken))
        {
            foreach (var provider in providers)
            {
                int providerId = provider.Id;

                providerToArtists[provider] = await dbContext.Artists
                    .AsNoTracking()
                    .Where(a => a.MusicProviderId == providerId)
                    .OrderBy(a => a.ArtistId)
                    .ToListAsync(stoppingToken);
            }
        }

        int totalAristsInDictionary = providerToArtists.Values.Sum(i => i.Count);

        if (totalAristsInDictionary == 0)
        {
            logger.LogWarning("Нет доступных артистов. Отмена поиска новинок");
            return Enumerable.Empty<NewAlbumFoundResult>().ToList();
        }
        logger.LogTrace("Запуск парсера по поиску новинок для {totalAristsInDictionary} артистов", totalAristsInDictionary);

        var list = new List<NewAlbumFoundResult>();
        bool isFirstRequest = true;

        foreach (var kvp in providerToArtists)
        {
            foreach (var artist in kvp.Value)
            {
                stoppingToken.ThrowIfCancellationRequested();

                // Пауза между запросами к сайтам, чтобы не словить блокировку
                if (!isFirstRequest)
                {
                    await Task.Delay(GetRequestDelay(), stoppingToken);
                }
                isFirstRequest = false;

                IReadOnlyList<AlbumDto> result;
                try
                {
                    result = await RefreshArtist(kvp.Key, artist.ArtistId, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    // Ошибка на одном артисте не должна останавливать обход всех остальных
                    logger.LogError(ex, "{providerName} -- Ошибка обновления артиста {artistName} (ID {artistId})",
                        kvp.Key.Name, artist.Name, artist.ArtistId);
                    continue;
                }

                if (result.Count == 0)
                {
                    continue;
                }

                list.Add(new NewAlbumFoundResult()
                {
                    ProviderName = kvp.Key.Name,
                    ArtistName = artist.Name,
                    ArtistUri = artist.Uri,
                    Albums = result
                });
            }
        }

        return list;
    }

    public async Task<IReadOnlyList<AlbumDto>> RefreshArtist(MusicProviderBase provider, int artistId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(provider);

        logger.LogInformation("Запуск поиска обновлений для артиста с id {artistId} провайдера {providerName}", artistId, provider.Name);

        Stopwatch sw = Stopwatch.StartNew();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var artist = await dbContext.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId, cancellationToken);
        if (artist == null)
        {
            logger.LogWarning("{providerName} -- Артист с ID {artistId} не найден", provider.Name, artistId);
            return Array.Empty<AlbumDto>();
        }

        var doc = await GetDocument(provider.AlbumsUriPath(artist), provider, cancellationToken);
        var albums = await provider.GetAlbumsAsync(artist, doc);

        // Для сравнения достаточно адресов уже сохранённых альбомов, сами альбомы в память не загружаем
        var knownAlbumUris = (await dbContext.Albums
            .AsNoTracking()
            .Where(a => a.ArtistId == artistId)
            .Select(a => a.Uri)
            .ToListAsync(cancellationToken))
            .ToHashSet();

        var newAlbums = new List<AlbumDto>();
        foreach (var album in albums)
        {
            // HashSet.Add заодно отсекает дубли внутри самой выдачи сайта
            if (!string.IsNullOrWhiteSpace(album.Uri) && knownAlbumUris.Add(album.Uri))
            {
                newAlbums.Add(album);
            }
        }

        if (newAlbums.Count > 0)
        {
            dbContext.Albums.AddRange(newAlbums.Select(album => album.ToEntity()));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        sw.Stop();

        logger.LogInformation("{providerName} -- Парсинг новинок артиста {artistName} (ID {artistId}) выполнен за [{parseDuration}]. Новых альбомов найдено: {newAlbumsCount}",
                provider.Name, artist.Name, artistId, sw.Elapsed, newAlbums.Count);

        return newAlbums;
    }

    public async Task RefreshAlbum(MusicProviderBase provider, int albumId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Запуск поиска обновлений для альбома с id {albumId} провайдера {providerName}", albumId, provider.Name);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var album = await dbContext.Albums.FirstOrDefaultAsync(a => a.AlbumId == albumId, cancellationToken);
        if (album == null)
        {
            logger.LogInformation("--{providerName} Альбом с ID [{albumId}] не найден",
                provider.Name, albumId);
            return;
        }

        var doc = await GetDocument(album.Uri, provider, cancellationToken);
        var tracks = await provider.GetTracksAsync(album, doc);

        if (tracks.Length == 0)
        {
            logger.LogInformation("Треки альбома '{albumName}' не найдены на сайте, сохранённые данные не изменены", album.Title);
            return;
        }

        // Уже сохранённые треки не удаляем (на них могут ссылаться записи истории загрузок), добавляем только новые
        var knownTrackUris = (await dbContext.Tracks
            .AsNoTracking()
            .Where(t => t.AlbumId == albumId && t.DownloadUri != null)
            .Select(t => t.DownloadUri!)
            .ToListAsync(cancellationToken))
            .ToHashSet();

        var newTracks = tracks
            .Where(track => string.IsNullOrWhiteSpace(track.DownloadUri) || knownTrackUris.Add(track.DownloadUri))
            .Select(track => track.ToEntity())
            .ToList();

        foreach (var track in newTracks)
        {
            track.AlbumId = albumId;
        }

        if (newTracks.Count > 0)
        {
            dbContext.Tracks.AddRange(newTracks);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation("Треки альбома '{albumName}' сохранены. Добавлено новых треков: {newTracksCount} (всего на сайте: {totalTracksCount})",
                album.Title, newTracks.Count, tracks.Length);
    }

    protected async Task<HtmlDocument> GetDocument(string uri, MusicProviderBase provider, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientProviderFactory.GetClientForProvider(provider);

            using var response = await client.GetAsync(uri, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                return doc;
            }

            logger.LogWarning("{providerName} -- Запрос {uri} завершился со статусом {statusCode}", provider.Name, uri, (int)response.StatusCode);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "{providerName} -- Не удалось получить страницу {uri}", provider.Name, uri);
        }

        return new HtmlDocument();
    }

    private TimeSpan GetRequestDelay()
    {
        var baseDelay = crawlerOptions.CurrentValue.RequestDelay;
        if (baseDelay <= TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        return baseDelay + TimeSpan.FromMilliseconds(Random.Shared.NextDouble() * baseDelay.TotalMilliseconds * 0.5);
    }
}
