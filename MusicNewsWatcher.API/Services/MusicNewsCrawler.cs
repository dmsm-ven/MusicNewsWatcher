using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.Models;
using System.Diagnostics;

namespace MusicNewsWatcher.API.Services;

public class MusicNewsCrawler
{
    private readonly IServiceScope services;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContextFactory;
    private readonly ILogger<MusicNewsCrawler> logger;

    public MusicNewsCrawler(IDbContextFactory<MusicWatcherDbContext> dbContextFactory, ILogger<MusicNewsCrawler> logger)
    {
        this.dbContextFactory = dbContextFactory;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders, CancellationToken stoppingToken)
    {
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
        {
            foreach (var artist in kvp.Value)
            {
                stoppingToken.ThrowIfCancellationRequested();

                var result = await CheckUpdatesForArtistAndSaveIfHasAsync(kvp.Key, artist.ArtistId);

                if (result != null && result.Any())
                {
                    list.Add(new NewAlbumFoundResult()
                    {
                        ProviderName = kvp.Key.Name,
                        ArtistName = artist.Name,
                        ArtistUri = artist.Uri,
                        Albums = result.Select(i => i.ToDto()).ToList()
                    });
                }
            }
        }

        return list;
    }
    public async Task<IReadOnlyList<AlbumEntity>> CheckUpdatesForArtistAndSaveIfHasAsync(MusicProviderBase provider, int artistId)
    {
        if (provider == null)
        {
            logger.LogError("Провайдер не может быть null");
            throw new ArgumentNullException(nameof(provider));
        }
        if (string.IsNullOrWhiteSpace(provider?.Name))
        {
            logger.LogError("Пустое имя провайдера (ID {providerId})", provider!.Id);
            throw new ArgumentNullException("Имя провайдера не может быть пустым");
        }

        Stopwatch sw = Stopwatch.StartNew();

        var dbContext = dbContextFactory.CreateDbContext();

        var artist = await dbContext.Artists.Include(a => a.Albums).FirstOrDefaultAsync(a => a.ArtistId == artistId);
        if (artist == null)
        {
            logger.LogWarning("{providerName} -- Артист с ID {artistId} не найден", provider.Name, artistId);
            return Array.Empty<AlbumEntity>();
        }

        var albums = await provider.GetAlbumsAsync(artist);

        var newAlbums = albums
            .Where(album => !string.IsNullOrWhiteSpace(album?.Uri))
            .Where(album => !artist.Albums.Any(existingAlbum => existingAlbum.Uri == album.Uri))
            .ToArray();

        if (newAlbums != null && newAlbums.Length > 0)
        {
            artist.Albums.AddRange(newAlbums);
            dbContext.Entry(artist).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();
        }

        sw.Stop();

        logger.LogTrace("{providerName} -- Парсинг новинок артиста {artistName} (ID {artistId}) выполнен за [{parseDuration}]. Новых альбомов найдено: {newAlbumsCount}",
                provider.Name, artist.Name, artistId, sw.Elapsed, (newAlbums?.Length ?? 0));

        return newAlbums ?? Enumerable.Empty<AlbumEntity>().ToArray();
    }
    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        var dbContext = dbContextFactory.CreateDbContext();

        var album = await dbContext.Albums.FindAsync(albumId);
        if (album != null)
        {
            var tracks = await provider.GetTracksAsync(album);

            int oldTracksCount = album.Tracks.Count;
            int newTracksCount = tracks.Count();

            album.Tracks.Clear();
            album.Tracks.AddRange(tracks);
            dbContext.Entry(album).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            logger.LogTrace("Треки альбома '{albumName}' сохранены. Было [{oldTracksCount}] стало [{newTracksCount}] треков",
                    album.Title, oldTracksCount, newTracksCount);
        }
        else
        {
            logger.LogWarning("--{providerName} Альбом с ID [{albumId}] не найден",
                provider.Name, albumId);
        }
    }
}
