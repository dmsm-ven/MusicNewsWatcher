using BandcampWatcher.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace BandcampWatcher.Models;

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
        await CheckUpdatesAll();
        OnUpdate?.Invoke();
    }

    public async Task CheckUpdatesAll()
    {
        foreach (var provider in musicProviders)
        {
            await CheckUpdatesForProvider(provider);
        }
    }

    public async Task CheckUpdatesForProvider(MusicProviderBase provider)
    {
        var providerArtists = dbContext.CreateDbContext()
            .MusicProviders.Include(p => p.Artists)
            .Single(p => p.MusicProviderId == provider.Id)
            .Artists;

        foreach (var artist in providerArtists)
        {
            await CheckUpdatesForArtist(artist);
        }
    }

    public async Task CheckUpdatesForArtist(ArtistEntity artist)
    {
        var provider = musicProviders.FirstOrDefault(p => p.Id == artist.MusicProviderId);
        var albums = await provider.GetAlbums(artist);

        await SaveChangesIfNeed(artist, albums);
    }

    private async Task SaveChangesIfNeed(ArtistEntity artist, List<AlbumEntity> lastAlbums)
    {
        using var db = await dbContext.CreateDbContextAsync();

        var dbArtist = await db.Artists.Include(a => a.Albums).SingleAsync(a => a.ArtistId == artist.ArtistId);
        var notAddedAlbums = lastAlbums.Where(a => !dbArtist.Albums.Select(a => a.Uri).Contains(a.Uri)).ToList();
        if (notAddedAlbums.Any())
        {
            dbArtist.Albums.AddRange(notAddedAlbums);
            db.SaveChanges();
        }
       
    }
}