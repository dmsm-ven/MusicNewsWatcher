using Microsoft.EntityFrameworkCore;
using System.Timers;

namespace MusicNewsWatcher.Core;

public class MusicUpdateManager : IDisposable
{
    public event EventHandler<NewAlbumsFoundEventArgs> OnNewAlbumsFound;

    public bool InProgress { get; private set; }

    private readonly List<MusicProviderBase> musicProviders;
    private readonly System.Timers.Timer autoUpdateTimer;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContext;  

    public MusicUpdateManager(IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory)
    {
        this.musicProviders = musicProviders.ToList();
        this.dbContext = dbContextFactory;
        autoUpdateTimer = new System.Timers.Timer();
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;       
    }

    public void Start(TimeSpan interval)
    {
        if(interval <= TimeSpan.FromMinutes(1))
        {
            throw new ArgumentOutOfRangeException(nameof(interval), "cannot update often than 1 min.");
        }
        autoUpdateTimer.Interval = (int)interval.TotalMilliseconds;
        autoUpdateTimer.Start();
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
        using var db = await dbContext.CreateDbContextAsync();
        
        var providerArtists = db.MusicProviders.Include(p => p.Artists)
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

        var artist = db.Artists.Include(x => x.Albums).Single(a => a.ArtistId == artistId);
        var albums = await provider.GetAlbumsAsync(artist);

        var notAddedAlbums = albums
            .Where(album => artist.Albums.FirstOrDefault(a => a.Uri.Equals(album.Uri, StringComparison.OrdinalIgnoreCase)) == null)
            .ToArray();

        if (notAddedAlbums.Any())
        {
            bool isFirstUpdate = artist.Albums.Count() == 0;

            artist.Albums.AddRange(notAddedAlbums);
            await db.SaveChangesAsync();

            OnNewAlbumsFound?.Invoke(this, new NewAlbumsFoundEventArgs()
            {
                Provider = provider.Name,
                Artist = new ArtistDto(artist.Name, artist.Uri),
                NewAlbums = notAddedAlbums.Select(a => new AlbumDto(a.Title, a.Uri)).ToArray()
            });
        }
    }

    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        using var db = await dbContext.CreateDbContextAsync();

        var album = db.Albums.Find(albumId);       
        var tracks = await provider.GetTracksAsync(album);

        album.Tracks.Clear();
        album.Tracks.AddRange(tracks);
        await db.SaveChangesAsync();
    }

    public void Dispose()
    {
        autoUpdateTimer.Elapsed -= AutoUpdateTimer_Elapsed;
        autoUpdateTimer.Stop();
        autoUpdateTimer.Dispose();
    }

    private async void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        await CheckUpdatesAllAsync();
    }
}





