using MusicNewsWatcher.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using MusicNewsWatcher.TelegramBot;
using System.Text;
using MusicNewsWatcher.Services;

namespace MusicNewsWatcher.Models;

public class MusicUpdateManager
{
    public event Action<AlbumEntity[]> OnUpdate;

    public bool InProgress { get; private set; }

    private readonly List<MusicProviderBase> musicProviders;
    private readonly Timer autoUpdateTimer;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContext;
    private readonly Func<MusicNewsWatcherTelegramBot> telegramBotFactory;
    private readonly IMusicNewsMessageFormatter telegramBotMessageFormatter;

    public MusicUpdateManager(
        IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory,
        Func<MusicNewsWatcherTelegramBot> telegramBotFactory,
        IMusicNewsMessageFormatter telegramBotMessageFormatter)
    {
        this.musicProviders = musicProviders.ToList();
        this.dbContext = dbContextFactory;
        this.telegramBotFactory = telegramBotFactory;
        this.telegramBotMessageFormatter = telegramBotMessageFormatter;
        autoUpdateTimer = new Timer((int)TimeSpan.FromMinutes(30).TotalMilliseconds);
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;
        autoUpdateTimer.Start();
    }

    private async void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        await CheckUpdatesAllAsync();       
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

            OnUpdate?.Invoke(notAddedAlbums);

            if (!isFirstUpdate)
            {
                await SendMessageToTelegramBot(provider, artist, notAddedAlbums);
            }
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

    private async Task SendMessageToTelegramBot(MusicProviderBase provider, ArtistEntity artist, IEnumerable<AlbumEntity> notAddedAlbums)
    {
        string message = telegramBotMessageFormatter.NewAlbumsFoundMessage(
            provider.Name,
            ValueTuple.Create(artist.Name, artist.Uri),
            notAddedAlbums.Select(a => ValueTuple.Create(a.Title, a.Uri)));

        using (var tgBot = telegramBotFactory())
        {
            await tgBot.SendAsBot(message);
        }
    }
}



