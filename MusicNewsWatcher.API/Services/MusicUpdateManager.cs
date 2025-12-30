using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.Core.Interfaces;
using MusicNewsWatcher.Core.Models;
using System.Diagnostics;

namespace MusicNewsWatcher.API.Services;

//TODO: разбить класс
public sealed class MusicUpdateManager(IEnumerable<MusicProviderBase> musicProviders,
        MusicWatcherDbContext dbContext,
        ILogger<MusicUpdateManager> logger,
        IMusicNewsCrawler crawler)
{
    public event EventHandler<NewAlbumsFoundEventArgs>? OnNewAlbumsFound;

    public static readonly string LastFullUpdateDateTimeSettingsKey = "LastFullUpdateDateTime";
    public static readonly TimeSpan DefaultTimerInterval = TimeSpan.FromMinutes(60);
    public static readonly TimeSpan DefaultMinInterval = TimeSpan.FromMinutes(1);

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
                logger.LogTrace("Обновление даты последнего переобхода [{lastUpdate}]", LastUpdate);
            }
        }
    }

    private TimeSpan updateInterval;
    public TimeSpan UpdateInterval
    {
        get => updateInterval;
        private set
        {

            if (value < DefaultMinInterval)
            {
                logger.LogWarning("Попытка назначить интервал обновления в недопустимом диапазоне ({value})", value);
                throw new ArgumentOutOfRangeException(nameof(UpdateInterval), "Сannot change update interval to les than 1 min.");
            }

            if (updateInterval != value)
            {
                updateInterval = value;
                logger.LogTrace("Интервал переобхода изменен на {updateInterval}", updateInterval);
            }
        }
    }

    private bool firstUpdate = true;

    /// <summary>
    /// Обновляет дату последнего переобхода
    /// </summary>
    /// <returns></returns>
    public async Task RefreshIntervalAndLastUpdate()
    {
        string? dateTimeStr = (await dbContext.Settings.FindAsync(LastFullUpdateDateTimeSettingsKey))?.Value;
        if (DateTimeOffset.TryParse(dateTimeStr, out var lastUpdate))
        {
            LastUpdate = lastUpdate;
        }
        else
        {
            LastUpdate = new DateTimeOffset();
        }

        TimeSpan newInterval = TimeSpan.Zero;

        string? intervalStr = (await dbContext.Settings.FindAsync("UpdateManagerIntervalInMinutes"))?.Value;
        if (int.TryParse(intervalStr, out var intervalMin))
        {
            newInterval = TimeSpan.FromMinutes(intervalMin);
        }
        else
        {
            //Если интервал не задан берем дефолтное хардкод значение
            newInterval = DefaultTimerInterval;
        }

        if (firstUpdate && LastUpdate + newInterval < DateTimeOffset.UtcNow)
        {
            newInterval = DefaultMinInterval;
        }

        UpdateInterval = newInterval;

        firstUpdate = false;
    }
    /// <summary>
    /// Выполяет переобход по всем сохраненным артистам для всех провайдеров и возращает общее количество новых найденных альбомов
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task<int> CheckUpdatesAllAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();

        int newAlbumsFound = 0;

        var newAlbumsByProvider = await crawler.CheckUpdatesAllAsync(stoppingToken);
        if (newAlbumsByProvider != null && newAlbumsByProvider.Any())
        {

            newAlbumsByProvider
                .Select(i => new NewAlbumsFoundEventArgs()
                {
                    Provider = i.ProviderName,
                    //Artist = new ArtistDto(i.id),
                    //NewAlbums = i.Albums.Select(al => new AlbumDto(al.Title, al.Uri)).ToArray()
                })
                .Where(i => i.NewAlbums != null && i.NewAlbums.Any())
                .ToList()
                .ForEach(i =>
                {
                    newAlbumsFound += i.NewAlbums!.Length;
                    OnNewAlbumsFound?.Invoke(this, i);
                });


        }

        LastUpdate = DateTimeOffset.UtcNow;

        await SaveLastUpdateTime();
        await RefreshIntervalAndLastUpdate();

        return newAlbumsFound;
    }
    public async Task CheckUpdatesForArtistForProvider(int providerId, int artistId, string artistName, string artistUri)
    {
        throw new NotImplementedException();
        /*
        var newAlbums = await crawler.CheckUpdatesForArtistAndSaveIfHasAsync(providerId, artistId);
        if (newAlbums != null && newAlbums.Any())
        {
            var e = new NewAlbumsFoundEventArgs()
            {
                Provider = provider.Name,
                Artist = new ArtistDto(artistName, artistUri),
                NewAlbums = newAlbums.Select(album => new AlbumDto(album.Title, album.Uri)).ToArray()
            };
            OnNewAlbumsFound?.Invoke(this, e);
        }*/
    }
    /// <summary>
    /// Заполняет информация по музыкальным трекам для альбома
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="albumId"></param>
    /// <returns></returns>
    public async Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId)
    {
        await crawler.CheckUpdatesForAlbumAsync(albumId);
    }
    private async Task SaveLastUpdateTime()
    {
        var item = await dbContext.Settings.FindAsync(LastFullUpdateDateTimeSettingsKey);

        if (item != null)
        {
            item.Value = LastUpdate.ToString();
        }
        else
        {
            var settingRecord = new SettingsEntity()
            {
                Name = LastFullUpdateDateTimeSettingsKey,
                Value = LastUpdate.ToString()
            };

            dbContext.Settings.Add(settingRecord);
        }

        await dbContext.SaveChangesAsync();

        logger.LogTrace("Дата последнего обновления сохранена. Новое значение: {updated}", LastUpdate);
    }
    public async Task RunCrawler(CancellationToken stoppingToken)
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

            var newAlbumsCount = await CheckUpdatesAllAsync(stoppingToken);

            const string message = "[{started}] - [{finished}] Переобход выполнен за {duration} сек. Найдено {newAlbumsCount} новых альбомов";
            logger.LogInformation(message,
                started,
                LastUpdate,
                (int)sw.Elapsed.TotalSeconds,
                newAlbumsCount);
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
}
