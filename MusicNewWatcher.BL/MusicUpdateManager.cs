using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Dto;
using MusicNewsWatcher.Core.Extensions;
using System.Diagnostics;
using System.Timers;

namespace MusicNewWatcher.BL;

//Переодически опрашивает сайты с треками и сохраняет результат, если в базе данных еще нет нового альбома для отслеживаемых исполнителей
//Запускается отдельно на консольном клиенте и на WPF Desktop приложении

//TODO: разбить класс
public sealed class MusicUpdateManager : IDisposable
{
    public event EventHandler<NewAlbumsFoundEventArgs>? OnNewAlbumsFound;

    public static readonly TimeSpan DefaultTimerInterval = TimeSpan.FromMinutes(60);

    public bool CrawlerInProgress { get; private set; }

    private DateTimeOffset lastUpdate;
    public DateTimeOffset LastUpdate
    {
        get => lastUpdate;
        private set
        {
            if (lastUpdate != value)
            {
                lastUpdate = value;
                logger.LogInformation("Обновление даты последнего переобхода [{lastUpdate}]", LastUpdate.ToLocalRuDateAndTime());
            }
        }
    }
    public TimeSpan UpdateInterval
    {
        get => TimeSpan.FromMilliseconds(autoUpdateTimer.Interval);
        private set
        {
            if (value != TimeSpan.Zero)
            {
                int newIntervalInMs = (int)value.TotalMilliseconds;

                if (value <= TimeSpan.FromMinutes(1))
                {
                    logger.LogInformation("Попытка назначить интервал обновления в недопустимом диапазоне ({newInterval}ms)", newIntervalInMs);
                    throw new ArgumentOutOfRangeException(nameof(UpdateInterval), "Сannot change update interval to les than 1 min.");
                }

                if (newIntervalInMs != (int)autoUpdateTimer.Interval)
                {
                    autoUpdateTimer.Interval = newIntervalInMs;
                    logger.LogInformation("Обновление даты последнего переобхода [{lastUpdate}]", LastUpdate.ToLocalRuDateAndTime());
                }
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
        TimeSpan startDelay = TimeSpan.FromSeconds(5);

        await RefreshInterval(initialRefresh: true);

        DateTimeOffset nextUpdate = LastUpdate + UpdateInterval;

        if (nextUpdate < DateTimeOffset.Now)
        {
            logger.LogInformation("Запуск парсера будет выполнен через ... {startDelay}", startDelay);
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
            logger.LogInformation("Следующий переобход будет запущен в: {time}", nextUpdate.ToLocalRuDateAndTime());
        }

        autoUpdateTimer.Start();
    }

    private async Task RefreshInterval(bool initialRefresh)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        string? dateTimeStr = (await db.Settings.FindAsync("LastFullUpdateDateTime"))?.Value;
        if (DateTimeOffset.TryParse(dateTimeStr, out var lastUpdate))
        {
            LastUpdate = lastUpdate;
        }
        else
        {
            LastUpdate = new DateTimeOffset();
        }

        string? intervalStr = (await db.Settings.FindAsync("UpdateManagerIntervalInMinutes"))?.Value;
        if (int.TryParse(intervalStr, out var intervalMin))
        {
            TimeSpan interval = TimeSpan.FromSeconds(intervalMin);
            bool isNotDefault = LastUpdate > DateTimeOffset.Now.Subtract(UpdateInterval);
            bool isEventInFuture = DateTimeOffset.Now.Subtract(LastUpdate) > UpdateInterval;


            if (initialRefresh && isNotDefault && isEventInFuture)
            {
                //Если первый запуск приложения и обновление было недавно (меньше целой части интервала обновления) то берем только часть интервала
                UpdateInterval = interval - (DateTimeOffset.Now - LastUpdate);
            }
            else
            {
                //Стандартный интервал через настройки
                UpdateInterval = interval;
            }
        }
        else
        {
            //Если интервал не задан берем дефолтное хардкод значение
            UpdateInterval = DefaultTimerInterval;
        }
    }

    public async Task CheckUpdatesAllAsync()
    {
        logger.LogInformation("Запуск переобхода парсером");

        var newAlbumsByProvider = await crawler.CheckUpdatesAllAsync(musicProviders);
        if (newAlbumsByProvider != null && newAlbumsByProvider.Any())
        {
            newAlbumsByProvider
                .Select(i => new NewAlbumsFoundEventArgs()
                {
                    Provider = i.ProviderName,
                    Artist = new ArtistDto(i.ArtistName, i.ArtistUri),
                    NewAlbums = i.Albums.Select(al => new AlbumDto(al.Title, al.Uri)).ToArray()
                })
                .Where(i => i.NewAlbums != null && i.NewAlbums.Any())
                .ToList()
                .ForEach(i => OnNewAlbumsFound?.Invoke(this, i));
        }

        LastUpdate = DateTimeOffset.UtcNow;
        await SaveLastUpdateTime();
    }

    public async Task CheckUpdatesForArtistForProvider(MusicProviderBase provider, int artistId, string artistName, string artistUri)
    {
        var newAlbums = await crawler.CheckUpdatesForArtistAndSaveIfHasAsync(provider, artistId);
        if (newAlbums != null && newAlbums.Any())
        {
            var e = new NewAlbumsFoundEventArgs()
            {
                Provider = provider.Name,
                Artist = new ArtistDto(artistName, artistUri),
                NewAlbums = newAlbums.Select(album => new AlbumDto(album.Title, album.Uri)).ToArray()
            };
            OnNewAlbumsFound?.Invoke(this, e);
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
        string settingsKey = "LastFullUpdateDateTime";

        var item = await db.Settings.FindAsync(settingsKey);

        if (item != null)
        {
            item.Value = LastUpdate.ToString();
        }
        else
        {
            var settingRecord = new SettingsEntity()
            {
                Name = settingsKey,
                Value = LastUpdate.ToString()
            };

            db.Settings.Add(settingRecord);

        }

        await db.SaveChangesAsync();

        logger.LogInformation("Дата последнего обновления сохранена. Новое значение: {updated}", LastUpdate.ToLocalRuDateAndTime());
    }

    private async void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            await RunCrawler();
            await RefreshInterval(initialRefresh: false);
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
            DateTimeOffset started = DateTimeOffset.UtcNow;

            await CheckUpdatesAllAsync();

            const string message = "Переобход по таймеру выполнен. [{started}] - [{LastUpdate}] (за {duration} сек.)";
            logger.LogInformation(message, started.ToLocalRuDateAndTime(), LastUpdate.ToLocalRuDateAndTime(), (int)sw.Elapsed.TotalSeconds);
        }
        catch (Exception ex)
        {
            logger.LogWarning("Ошибка переобхода по таймеру: {error}", ex.Message);
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
