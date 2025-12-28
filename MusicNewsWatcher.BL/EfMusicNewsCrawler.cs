using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.DataAccess.Entity;
using System.Diagnostics;

namespace MusicNewsWatcher.BL;

public class EfMusicNewsCrawler : IMusicNewsCrawler
{
    private readonly MusicWatcherDbContext dbContext;
    private readonly ILogger<EfMusicNewsCrawler> logger;

    public EfMusicNewsCrawler(MusicWatcherDbContext dbContext, ILogger<EfMusicNewsCrawler> logger)
    {
        this.dbContext = dbContext;
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


        foreach (var provider in musicProviders)
        {
            var providerArtists = dbContext.MusicProviders
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
                        Albums = result
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

        var artist = await dbContext.Artists.Include(a => a.Albums).FirstOrDefaultAsync(a => a.ArtistId == artistId);
        if (artist == null)
        {
            logger.LogWarning("{providerName} -- Артист с ID {artistId} не найден", provider.Name, artistId);
            return Enumerable.Empty<AlbumEntity>().ToArray();
        }

        var albums = await provider.GetAlbumsAsync(artist);

        var newAlbums = albums
            .Where(album => album != null && !string.IsNullOrWhiteSpace(album.Uri))
            .ExceptByProperty(artist.Albums, album => album.Uri)
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