using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewWatcher.BL;

public class EfMusicNewsCrawler : IMusicNewsCrawler
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly ILogger<EfMusicNewsCrawler> logger;

    public EfMusicNewsCrawler(IDbContextFactory<MusicWatcherDbContext> dbFactory, ILogger<EfMusicNewsCrawler> logger)
    {
        this.dbFactory = dbFactory;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders)
    {
        if (musicProviders == null || musicProviders.Count() == 0)
        {
            return Enumerable.Empty<NewAlbumFoundResult>().ToList();
        }

        var providerToArtists = new Dictionary<MusicProviderBase, List<ArtistEntity>>();

        using (var db = await dbFactory.CreateDbContextAsync())
        {
            foreach (var provider in musicProviders)
            {
                var providerArtists = db.MusicProviders
                    .Include(i => i.Artists)
                    .ThenInclude(a => a.Albums)
                    .Single(p => p.MusicProviderId == provider.Id)
                    .Artists
                    .ToList();

                providerToArtists[provider] = providerArtists;
            }
        }

        if (providerToArtists.Count == 0)
        {
            return Enumerable.Empty<NewAlbumFoundResult>().ToList();
        }

        var list = new List<NewAlbumFoundResult>();

        foreach (var kvp in providerToArtists)
        {
            foreach (var artist in kvp.Value)
            {
                var result = await CheckUpdatesForArtistAndSaveIfHasAsync(kvp.Key, artist.ArtistId);
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

    public async Task<IReadOnlyList<AlbumEntity>> CheckUpdatesForArtistAndSaveIfHasAsync(MusicProviderBase provider, int artistId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var artist = await db.Artists.Include(a => a.Albums).FirstOrDefaultAsync(a => a.ArtistId == artistId);
        if (artist == null)
        {
            return Enumerable.Empty<AlbumEntity>().ToArray();
        }

        var albums = await provider.GetAlbumsAsync(artist);

        var newAlbums = albums
            .Where(album => album != null && !string.IsNullOrWhiteSpace(album.Uri))
            .ExceptByProperty(artist.Albums, album => album.Uri)
            .ToArray();

        if (newAlbums.Length == 0)
        {
            return Enumerable.Empty<AlbumEntity>().ToArray();
        }

        db.Artists.Attach(artist);
        db.Entry(artist).State = EntityState.Modified;
        artist.Albums.AddRange(newAlbums);

        await db.SaveChangesAsync();

        foreach (var album in newAlbums)
        {
            logger.LogInformation("Парсер: Найден новый альбом '{albumName}' у исполнителя '{artistName}' ({providerName})",
                album.Title, artist.Name, provider.Name);
        }

        return newAlbums;
    }

    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var album = await db.Albums.FindAsync(albumId);
        if (album != null)
        {
            var tracks = await provider.GetTracksAsync(album);

            album.Tracks.Clear();
            album.Tracks.AddRange(tracks);

            await db.SaveChangesAsync();
        }
    }
}