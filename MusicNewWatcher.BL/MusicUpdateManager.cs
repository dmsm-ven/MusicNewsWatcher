﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Dto;
using MusicNewsWatcher.Core.Extensions;
using System.Diagnostics;

namespace MusicNewWatcher.BL;

//Переодически опрашивает сайты с треками и сохраняет результат, если в базе данных еще нет нового альбома для отслеживаемых исполнителей
//Запускается отдельно на консольном клиенте и на WPF Desktop приложении

//TODO: разбить класс
public sealed class MusicUpdateManager
{
    public event EventHandler<NewAlbumsFoundEventArgs>? OnNewAlbumsFound;

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
                logger.LogTrace("Обновление даты последнего переобхода [{lastUpdate}]", LastUpdate.ToLocalRuDateAndTime());
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

    private readonly List<MusicProviderBase> musicProviders;
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
    }

    public async Task RefreshIntervalAndLastUpdate()
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

        TimeSpan newInterval = TimeSpan.Zero;

        string? intervalStr = (await db.Settings.FindAsync("UpdateManagerIntervalInMinutes"))?.Value;
        if (int.TryParse(intervalStr, out var intervalMin))
        {
            newInterval = TimeSpan.FromMinutes(intervalMin);
        }
        else
        {
            //Если интервал не задан берем дефолтное хардкод значение
            newInterval = DefaultTimerInterval;
        }

        if (LastUpdate + newInterval < DateTimeOffset.Now)
        {
            newInterval = DefaultMinInterval;
        }

        UpdateInterval = newInterval;
    }

    public async Task CheckUpdatesAllAsync(CancellationToken stoppingToken)
    {
        var newAlbumsByProvider = await crawler.CheckUpdatesAllAsync(musicProviders, stoppingToken);
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

        logger.LogTrace("Дата последнего обновления сохранена. Новое значение: {updated}", LastUpdate.ToLocalRuDateAndTime());
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

            await CheckUpdatesAllAsync(stoppingToken);

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
            await RefreshIntervalAndLastUpdate();
            CrawlerInProgress = false;
        }
    }
}
