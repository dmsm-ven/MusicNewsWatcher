using MusicNewsWatcher.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace MusicNewsWatcher.Models;

public class MusicManager
{
    public event Action OnUpdate;

    public bool InProgress { get; private set; }

    private readonly List<MusicProviderBase> musicProviders;
    private readonly Timer autoUpdateTimer;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContext;

    public MusicManager(IEnumerable<MusicProviderBase> musicProviders, IDbContextFactory<MusicWatcherDbContext> dbContext)
    {
        this.musicProviders = musicProviders.ToList();
        this.dbContext = dbContext;

        autoUpdateTimer = new Timer((int)TimeSpan.FromHours(1).TotalMilliseconds);
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;
        autoUpdateTimer.Start();      
    }

    private async void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        await CheckUpdatesAllAsync();
        OnUpdate?.Invoke();
    }

    public async Task CheckUpdatesAllAsync()
    {
        foreach (var provider in musicProviders)
        {
            await CheckUpdatesForProviderAsync(provider);
        }
    }

    public async Task CheckUpdatesForProviderAsync(MusicProviderBase provider)
    {
        var providerArtists = dbContext.CreateDbContext()
            .MusicProviders.Include(p => p.Artists)
            .Single(p => p.MusicProviderId == provider.Id)
            .Artists;

        foreach (var artist in providerArtists)
        {
            await CheckUpdatesForArtistAsync(provider, artist.ArtistId);
        }
    }

    public async Task CheckUpdatesForArtistAsync(MusicProviderBase provider, int artistId)
    {
        using var db = await dbContext.CreateDbContextAsync();

        var artist = db.Artists.Find(artistId);
        var albums = await provider.GetAlbumsAsync(artist);

        await SaveChangesIfNeedAsync(db, artist, albums);
    }

    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        using var db = await dbContext.CreateDbContextAsync();

        var album = db.Albums.Find(albumId);       
        var tracks = await provider.GetTracksAsync(album);

        album.Tracks.Clear();
        album.Tracks.AddRange(tracks);
        db.SaveChanges();
    }

    private async Task SaveChangesIfNeedAsync(MusicWatcherDbContext db, ArtistEntity artist, IEnumerable<AlbumEntity> lastAlbums)
    {
        var notAddedAlbums = lastAlbums.Where(a => !artist.Albums.Select(a => a.Uri).Contains(a.Uri)).ToList();

        if (notAddedAlbums.Any())
        {            
            artist.Albums.AddRange(notAddedAlbums);
            db.SaveChanges();
        }
    }
}