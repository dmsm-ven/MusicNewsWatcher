using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Timers;

namespace MusicNewsWatcher.Core;

//Переодически опрашивает сайты с треками и сохраняет результат, если в базе данных еще нет нового альбома для отслеживаемых исполнителей
//Запускается отдельно на консольном клиенте и на WPF Desktop приложении

//TODO: разбить класс
public class MusicUpdateManager : IDisposable
{
    public event EventHandler<NewAlbumsFoundEventArgs> OnNewAlbumsFound;

    public bool CrawlerInProgress { get; private set; }
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
            if (inMs != autoUpdateTimer.Interval)
            {
                autoUpdateTimer.Interval = inMs;
            }
        }
    }

    private readonly List<MusicProviderBase> musicProviders;
    private readonly System.Timers.Timer autoUpdateTimer;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly ILogger<MusicUpdateManager> logger;

    public MusicUpdateManager(IEnumerable<MusicProviderBase> musicProviders,
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory,
        ILogger<MusicUpdateManager> logger)
    {
        this.musicProviders = musicProviders.ToList();
        this.dbFactory = dbContextFactory;
        this.logger = logger;
        autoUpdateTimer = new System.Timers.Timer();
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;
    }

    public async Task Start()
    {
        logger.LogInformation("Запуск парсера");
        TimeSpan startDelay = TimeSpan.FromSeconds(5);

        RefreshInterval();
        autoUpdateTimer.Start();

        //При запуске приложения проверяем когда был последний запуск.
        //Если больше чем интервал, то запускаем проверку сразу после небольшой задержки
        if (LastUpdate + UpdateInterval < DateTime.Now)
        {
            logger.LogInformation($"Запуск переобхода через {(int)startDelay.TotalSeconds} секунд ...");

            await Task.Delay(startDelay);

            AutoUpdateTimer_Elapsed(this, null);
        }

        logger.LogInformation("Парсер запущен");

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
        logger.LogInformation("Запуск переобхода парсером");

        CrawlerInProgress = true;

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
                await CheckUpdatesForArtistAsync(kvp.Key, artist.ArtistId);
            }
        }

        CrawlerInProgress = false;
    }

    public async Task CheckUpdatesForArtistAsync(MusicProviderBase provider, int artistId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var artist = await db.Artists.FindAsync(artistId);
        if (artist == null)
        {
            return;
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
            return;
        }

        var entity = db.Artists.Attach(artist);

        artist.Albums.AddRange(newAlbums);
        entity.State = EntityState.Modified;

        await db.SaveChangesAsync();

        OnNewAlbumsFound?.Invoke(this, new NewAlbumsFoundEventArgs()
        {
            Provider = provider.Name,
            Artist = new ArtistDto(artist.Name, artist.Uri),
            NewAlbums = newAlbums.Select(a => new AlbumDto(a.Title, a.Uri)).ToArray()
        });
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

        var album = await db.Albums.FindAsync(albumId);
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

        var item = await db.Settings.FindAsync(settingsKey);

        if (item != null)
        {
            item.Value = LastUpdate.ToString(dtFormat);
        }
        else
        {
            var settingRecord = new SettingsEntity()
            {
                Name = settingsKey,
                Value = LastUpdate.ToString(dtFormat)
            };

            db.Settings.Add(settingRecord);

        }

        await db.SaveChangesAsync();

        logger.LogInformation($"Дата последнего обновления сохранена. Новое значение: {LastUpdate.ToString(dtFormat)}");
    }

    private async void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        if (CrawlerInProgress)
        {
            return;
        }

        Stopwatch sw = Stopwatch.StartNew();
        DateTime started = DateTime.UtcNow;

        await CheckUpdatesAllAsync();
        LastUpdate = DateTime.UtcNow;
        await SaveLastUpdateTime();
        RefreshInterval();

        var sb = new StringBuilder()
            .Append("Переобход по таймеру выполнен. ")
            .Append($"Начало: [{started.ToShortTimeString()}] | ")
            .Append($"Конец [{LastUpdate.ToShortTimeString()}] | ")
            .Append($"Длительность выполнения: {(int)sw.Elapsed.TotalSeconds} сек.");

        logger.LogInformation(sb.ToString());

    }

    public void Dispose()
    {
        if (autoUpdateTimer != null)
        {
            autoUpdateTimer.Elapsed -= AutoUpdateTimer_Elapsed;
            autoUpdateTimer.Stop();
            autoUpdateTimer.Dispose();
        }
    }
}