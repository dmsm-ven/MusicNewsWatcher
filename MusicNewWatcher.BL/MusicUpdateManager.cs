using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Timers;

namespace MusicNewWatcher.BL;

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
    private readonly IMusicNewsCrawler crawler;

    public MusicUpdateManager(IEnumerable<MusicProviderBase> musicProviders,
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory,
        ILogger<MusicUpdateManager> logger,
        IMusicNewsCrawler crawler)
    {
        this.musicProviders = musicProviders.ToList();
        dbFactory = dbContextFactory;
        this.logger = logger;
        this.crawler = crawler;
        autoUpdateTimer = new System.Timers.Timer();
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;
    }

    public async Task Start()
    {
        logger.LogInformation("Запуск парсера");
        TimeSpan startDelay = TimeSpan.FromSeconds(5);

        //При запуске приложения проверяем когда был последний запуск.
        //Если нужно, то запускаем проверку сразу после небольшой задержки при старте приложения
        if (LastUpdate + UpdateInterval < DateTime.Now)
        {
            await Task.Delay(startDelay);
            try
            {
                await RunCrawler();
            }
            catch
            {
                throw;
            }
        }
        else
        {
            logger.LogInformation($"Следующий переобход будет запущен в: [{(LastUpdate + UpdateInterval).ToShortTimeString()}]");
        }

        //И запускаем таймер проверки
        autoUpdateTimer.Start();
    }

    private async Task RefreshInterval()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        string intervalStr = (await db.Settings.FindAsync("UpdateManagerIntervalInMinutes"))?.Value;
        string dateTimeStr = (await db.Settings.FindAsync("LastFullUpdateDateTime"))?.Value;

        UpdateInterval = TimeSpan.FromMinutes(int.Parse(intervalStr ?? "30"));
        LastUpdate = DateTime.Parse(dateTimeStr ?? DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"), new CultureInfo("ru-RU"));
    }

    public async Task CheckUpdatesAllAsync()
    {
        logger.LogInformation("Запуск переобхода парсером");

        await crawler.CheckUpdatesAllAsync(musicProviders);

        LastUpdate = DateTime.UtcNow;
        await SaveLastUpdateTime();
        await RefreshInterval();
    }

    public async Task CheckUpdatesForArtistForProvider(MusicProviderBase provider, int artistId, string artistName, string artistUri)
    {
        var newAlbums = await crawler.CheckUpdatesForArtistAndSaveIfHasAsync(provider, artistId);

        if (newAlbums.Any())
        {
            OnNewAlbumsFound?.Invoke(this, new NewAlbumsFoundEventArgs()
            {
                Provider = provider.Name,
                Artist = new ArtistDto(artistName, artistUri),
                NewAlbums = newAlbums.Select(a => new AlbumDto(a.Title, a.Uri)).ToArray()
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
        await crawler.CheckUpdatesForAlbumAsync(provider, albumId);
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


        try
        {
            await RunCrawler();
        }
        catch
        {
            throw;
        }
    }

    private async Task RunCrawler()
    {
        if (CrawlerInProgress)
        {
            logger.LogWarning("Попытка выполнения переобхода во время выполнения другого (Отмена)");
            return;
        }

        CrawlerInProgress = true;

        try
        {

            Stopwatch sw = Stopwatch.StartNew();
            DateTime started = DateTime.UtcNow;

            await CheckUpdatesAllAsync();

            var sb = new StringBuilder()
                .Append("Переобход по таймеру выполнен. ")
                .Append($"Начало: [{started.ToShortTimeString()}] | ")
                .Append($"Конец [{LastUpdate.ToShortTimeString()}] | ")
                .Append($"Длительность выполнения: {(int)sw.Elapsed.TotalSeconds} сек.");

            logger.LogInformation(sb.ToString());
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Ошибка переобхода по таймеру: {ex.Message}");
            throw;
        }
        finally
        {
            CrawlerInProgress = false;
        }
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
