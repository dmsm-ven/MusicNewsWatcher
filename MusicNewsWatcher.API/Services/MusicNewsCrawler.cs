using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Core.Models.Dtos;
using System.Diagnostics;

namespace MusicNewsWatcher.API.Services;

public class MusicNewsCrawler(IDbContextFactory<MusicWatcherDbContext> dbContextFactory,
        CrawlerHttpClientProviderFactory httpClientProviderFactory,
        ILogger<MusicNewsCrawler> logger)
{
    public async Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders, CancellationToken stoppingToken)
    {
        logger.LogInformation("Запуск поиска обновлений всех провайдеров");

        stoppingToken.ThrowIfCancellationRequested();

        if (musicProviders == null || musicProviders.Count() == 0)
        {
            logger.LogWarning("Нет доступных провайдеров. Отмена поиска новинок");
            return Enumerable.Empty<NewAlbumFoundResult>().ToList();
        }

        logger.LogTrace("Запуск парсера по поиску новинок для всех провайдеров ({providerNames})",
        string.Join(", ", musicProviders.Select(m => m.Name)));

        var providerToArtists = new Dictionary<MusicProviderBase, List<ArtistEntity>>();

        using var dbContext = dbContextFactory.CreateDbContext();

        foreach (var provider in musicProviders)
        {
            var providerArtists = dbContext.MusicProviders
                .AsNoTrackingWithIdentityResolution()
                .Include(i => i.Artists)
                .ThenInclude(a => a.Albums)
                .Single(p => p.MusicProviderId == provider.Id)
                .Artists
                .ToList();

            providerToArtists[provider] = providerArtists;
        }

        int totalAristsInDictionary = providerToArtists.Values.Sum(i => i.Count);

        if (totalAristsInDictionary == 0)
        {
            logger.LogWarning("Нет доступных артистов. Отмена поиска новинок");
            return Enumerable.Empty<NewAlbumFoundResult>().ToList();
        }
        logger.LogTrace("Запуск парсера по поиску новинок для {totalAristsInDictionary} артистов", totalAristsInDictionary);


        var list = new List<NewAlbumFoundResult>();

        foreach (var kvp in providerToArtists)
            foreach (var artist in kvp.Value)
            {
                stoppingToken.ThrowIfCancellationRequested();

                var result = await RefreshArtist(kvp.Key, artist.ArtistId);

                if ((result?.Count ?? 0) == 0)
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


        return list;
    }
    public async Task<IReadOnlyList<AlbumDto>> RefreshArtist(MusicProviderBase provider, int artistId)
    {
        logger.LogInformation("Запуск поиска обновлений для артиста с id {artistId} провайдера {providerName}", artistId, provider.Name);

        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        Stopwatch sw = Stopwatch.StartNew();
        var dbContext = dbContextFactory.CreateDbContext();

        var artist = await dbContext.Artists.Include(a => a.Albums).FirstOrDefaultAsync(a => a.ArtistId == artistId);
        if (artist == null)
        {
            logger.LogWarning("{providerName} -- Артист с ID {artistId} не найден", provider.Name, artistId);
            return Array.Empty<AlbumDto>();
        }

        var doc = await GetDocument(provider.AlbumsUriPath(artist), provider);
        var albums = await provider.GetAlbumsAsync(artist, doc);

        var newAlbums = albums
            .Where(album => !string.IsNullOrWhiteSpace(album?.Uri))
            .Where(album => !artist.Albums.Any(existingAlbum => existingAlbum.Uri == album.Uri))
            .ToArray();

        if (newAlbums != null && newAlbums.Length > 0)
        {
            artist.Albums.AddRange(newAlbums.Select(album => album.ToEntity()));
            dbContext.Entry(artist).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();
        }

        sw.Stop();

        logger.LogInformation("{providerName} -- Парсинг новинок артиста {artistName} (ID {artistId}) выполнен за [{parseDuration}]. Новых альбомов найдено: {newAlbumsCount}",
                provider.Name, artist.Name, artistId, sw.Elapsed, (newAlbums?.Length ?? 0));

        return newAlbums ?? Array.Empty<AlbumDto>();
    }
    public async Task RefreshAlbum(MusicProviderBase provider, int albumId)
    {
        logger.LogInformation("Запуск поиска обновлений для альбома с id {albumId} провайдера {providerName}", albumId, provider.Name);

        var dbContext = dbContextFactory.CreateDbContext();

        var album = await dbContext.Albums.FindAsync(albumId);
        if (album != null)
        {
            var doc = await GetDocument(album.Uri, provider);
            var tracks = await provider.GetTracksAsync(album, doc);

            int oldTracksCount = album.Tracks.Count;
            int newTracksCount = tracks.Count();

            album.Tracks.Clear();
            album.Tracks.AddRange(tracks.Select(track => track.ToEntity()));
            dbContext.Entry(album).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            logger.LogInformation("Треки альбома '{albumName}' сохранены. Было [{oldTracksCount}] стало [{newTracksCount}] треков",
                    album.Title, oldTracksCount, newTracksCount);
        }
        else
        {
            logger.LogInformation("--{providerName} Альбом с ID [{albumId}] не найден",
                provider.Name, albumId);
        }
    }
    protected async Task<HtmlDocument?> GetDocument(string uri, MusicProviderBase provider)
    {
        try
        {
            var response = await httpClientProviderFactory.GetClientForProvider(provider).GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                return doc;
            }
        }
        catch
        {

        }
        return null;
    }
}
