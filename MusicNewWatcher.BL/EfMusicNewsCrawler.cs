using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewWatcher.BL;

public class EfMusicNewsCrawler : IMusicNewsCrawler
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;

    public EfMusicNewsCrawler(IDbContextFactory<MusicWatcherDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public async Task CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders)
    {
        if (musicProviders == null || musicProviders.Count() == 0)
        {
            return;
        }

        var proviiderToArtists = new Dictionary<MusicProviderBase?, List<ArtistEntity>>();

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

                proviiderToArtists[provider] = providerArtists;
            }
        }

        foreach (var kvp in proviiderToArtists)
        {
            foreach (var artist in kvp.Value)
            {
                await CheckUpdatesForArtistAndSaveIfHasAsync(kvp.Key, artist.ArtistId);
            }
        }
    }

    public async Task<AlbumEntity[]> CheckUpdatesForArtistAndSaveIfHasAsync(MusicProviderBase provider, int artistId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var artist = await db.Artists.FindAsync(artistId);
        if (artist == null)
        {
            return Enumerable.Empty<AlbumEntity>().ToArray();
        }

        var albums = await provider.GetAlbumsAsync(artist);

        //var newAlbums = albums
        //    .Where(album => !string.IsNullOrWhiteSpace(album.Uri))
        //    .Where(album => artist.Albums.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Uri) && a.Uri.Equals(album.Uri, StringComparison.OrdinalIgnoreCase)) == null)
        //    .ToArray();

        var newAlbums = artist.Albums
            .Where(album => album != null && !string.IsNullOrWhiteSpace(album.Uri))
            .ExceptByProperty(albums, album => album.Uri)
            .ToArray();

        if (newAlbums.Length == 0)
        {
            return Enumerable.Empty<AlbumEntity>().ToArray();
        }

        var entity = db.Artists.Attach(artist);

        artist.Albums.AddRange(newAlbums);
        entity.State = EntityState.Modified;

        await db.SaveChangesAsync();

        return newAlbums;
    }

    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var album = await db.Albums.FindAsync(albumId);
        var tracks = await provider.GetTracksAsync(album);

        album.Tracks.Clear();
        album.Tracks.AddRange(tracks);

        await db.SaveChangesAsync();
    }
}