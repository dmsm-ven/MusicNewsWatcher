using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Timers;

namespace MusicNewsWatcher.Core;

//Переодически опрашивает сайты музыки и сохраняет результат, если в базе данных еще нет нового альбома для отслеживаемых исполнителей
//Запускается отдельно на консольном клиенте и на WPF Desktop приложении
public class MusicUpdateManager : IDisposable
{
    public event EventHandler<NewAlbumsFoundEventArgs> OnNewAlbumsFound;

    public bool InProgress { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TimeSpan UpdateInterval
    {
        get => TimeSpan.FromMilliseconds(autoUpdateTimer.Interval);
        private set
        {
            double inMs = (int)value.TotalMilliseconds;
            if (value <= TimeSpan.FromMinutes(1))
            {
                throw new ArgumentOutOfRangeException(nameof(UpdateInterval), "cannot update often than 1 min.");
            }
            if(inMs != autoUpdateTimer.Interval)
            {
                autoUpdateTimer.Interval = inMs;
            }
        }
    }

    private readonly List<MusicProviderBase> musicProviders;
    private readonly System.Timers.Timer autoUpdateTimer;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;  

    public MusicUpdateManager(IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory)
    {
        this.musicProviders = musicProviders.ToList();
        this.dbFactory = dbContextFactory;
        autoUpdateTimer = new System.Timers.Timer();
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;       
    }

    public async void Start()
    {
        RefreshInterval();        
        autoUpdateTimer.Start();
        
        //При запуске приложения проверяем когда был последний запуск.
        //Если больше чем интервал, то запускаем проверку сразу (после небольшой задержки)
        if(LastUpdate + UpdateInterval < DateTime.Now)
        {
            const int delayInSeconds = 5;
            WriteMessageWithTime($"Проверка запустить через {delayInSeconds} секунд ...");

            await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));

            AutoUpdateTimer_Elapsed(this, null);
        }
    }

    private void RefreshInterval()
    {
        using (var db = dbFactory.CreateDbContext())
        {
            string intervalStr = db.Settings.Find("UpdateManagerIntervalInMinutes")?.Value;
            string dateTimeStr = db.Settings.Find("LastFullUpdateDateTime")?.Value;

            UpdateInterval = TimeSpan.FromMinutes(int.Parse(intervalStr ?? "30"));
            LastUpdate = DateTime.Parse(dateTimeStr ?? DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"), new CultureInfo("ru-RU"));
        }
    }

    public async Task CheckUpdatesAllAsync()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        foreach (var provider in musicProviders)
        {
            var providerArtists = db.MusicProviders
                .Include(i => i.Artists)
                .ThenInclude(a => a.Albums)
                .Single(p => p.MusicProviderId == provider.Id)
                .Artists;

            foreach (var artist in providerArtists)
            {
                await CheckUpdatesForArtistAsync(db, provider, artist);
            }
        }       
    }
    
    public async Task CheckUpdatesForArtistAsync(MusicProviderBase provider, int artistId)
    {
        using(var db = await dbFactory.CreateDbContextAsync())
        {
            var artist = db.Artists.Include(x => x.Albums).Single(a => a.ArtistId == artistId);

            await CheckUpdatesForArtistAsync(db, provider, artist);
        }
    }

    public async Task CheckUpdatesForArtistAsync(MusicWatcherDbContext db, MusicProviderBase provider, ArtistEntity artist)
    {
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

    /// <summary>
    /// Заполняет информация по музыкальным трекам для альбома
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var album = db.Albums.Find(albumId);       
        var tracks = await provider.GetTracksAsync(album);

        album.Tracks.Clear();
        album.Tracks.AddRange(tracks);
        await db.SaveChangesAsync();
    }

    private async Task SaveLastUpdateTime()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        string dtFormat = "dd.MM.yyyy HH:mm:ss";
        string settingsKey = "LastFullUpdateDateTime";

        var item = db.Settings.Find(settingsKey);
        if (item != null)
        {
            item.Value = LastUpdate.ToString(dtFormat);
        }
        else
        {
            db.Settings.Add(new SettingsEntity() { Name = settingsKey, Value = LastUpdate.ToString(dtFormat) });
        }
        db.SaveChanges();     
    }

    private async void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        Stopwatch sw = Stopwatch.StartNew();
        WriteMessageWithTime("Переобход запущен...");

        await CheckUpdatesAllAsync();
        LastUpdate = DateTime.Now;
        await SaveLastUpdateTime();
        RefreshInterval();

        WriteMessageWithTime($"Переобход закончен. Длительность выполнения: {(int)sw.Elapsed.TotalSeconds} сек.");
    }

    private static void WriteMessageWithTime(string? message, ConsoleColor? color = null)
    {
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
        }

        Console.WriteLine($"[{DateTime.Now.ToString()}] {message ?? "<пусто>"}");

        if (color.HasValue)
        {
            Console.ResetColor();
        }
    }

    public void Dispose()
    {
        autoUpdateTimer.Elapsed -= AutoUpdateTimer_Elapsed;
        autoUpdateTimer.Stop();
        autoUpdateTimer.Dispose();
    }
}





