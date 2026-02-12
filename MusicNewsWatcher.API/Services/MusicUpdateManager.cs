using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Core.Models.Dtos;
using System.Diagnostics;

namespace MusicNewsWatcher.API.Services;

//TODO: разбить класс
public sealed class MusicUpdateManager(IEnumerable<MusicProviderBase> musicProviders,
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory,
        ILogger<MusicUpdateManager> logger,
        MusicNewsCrawler crawler)
{
    public event EventHandler<NewAlbumsFoundEventArgs[]>? NewAlbumsFound;

    public static readonly string LastFullUpdateDateTimeSettingsKey = "LastFullUpdateDateTime";
    public bool CrawlerInProgress { get; private set; }
    public DateTimeOffset LastUpdate { get; private set; }
    public bool IsFirstUpdate { get; private set; } = true;

    /// <summary>
    /// Обновляет дату последнего переобхода
    /// </summary>
    /// <returns></returns>
    public async Task RefreshLastUpdateDateTime()
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();

        string? dateTimeStr = (await dbContext.Settings.FindAsync(LastFullUpdateDateTimeSettingsKey))?.Value;
        if (DateTimeOffset.TryParse(dateTimeStr, out var lastUpdate))
        {
            LastUpdate = lastUpdate;
        }
        else
        {
            LastUpdate = new DateTimeOffset();
        }

        IsFirstUpdate = false;
    }
    /// <summary>
    /// Выполяет переобход по всем сохраненным артистам для всех провайдеров и возращает общее количество новых найденных альбомов
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task<NewAlbumsFoundEventArgs[]> CheckUpdatesAllAsync(CancellationToken stoppingToken)
    {
        var result = new List<NewAlbumsFoundEventArgs>();

        var newAlbumsByProvider = await crawler.CheckUpdatesAllAsync(musicProviders, stoppingToken);
        if (newAlbumsByProvider != null && newAlbumsByProvider.Any())
        {
            var data = newAlbumsByProvider
                    .Select(i => new NewAlbumsFoundEventArgs()
                    {
                        Provider = i.ProviderName,
                        Artist = new ArtistDto() { Name = i.ArtistName, Uri = i.ArtistUri },
                        NewAlbums = i.Albums.ToArray(),
                    })
                    .Where(i => i.NewAlbums != null && i.NewAlbums.Any())
                    .ToArray();

            result.AddRange(data);
        }

        LastUpdate = DateTimeOffset.UtcNow;

        await SaveLastUpdateTime();
        await RefreshLastUpdateDateTime();

        return result.ToArray();
    }
    public async Task CheckUpdatesForArtistAsync(int providerId, int artistId)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();

        MusicProviderBase provider = musicProviders.Single(p => p.Id == providerId);
        var artist = await dbContext.Artists.FindAsync(artistId);

        logger.LogInformation("Начало получения обновлений альбомов для артиста {artist}", artist.Name);
        var newAlbums = await crawler.RefreshArtist(provider, artistId);
        logger.LogInformation("Получены альбомы для артиста {artist} ({count} альбомов)", artist.Name, newAlbums.Count);
    }
    public async Task CheckUpdatesForAlbumAsync(int providerId, int albumId)
    {
        MusicProviderBase provider = musicProviders.Single(p => p.Id == providerId);
        await crawler.RefreshAlbum(provider, albumId);
    }
    public async Task RunCrawler(CancellationToken stoppingToken = default)
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

            logger.LogInformation("Начало переообхода на поиск новинок музыкальных альбомов");

            var result = await CheckUpdatesAllAsync(stoppingToken);

            const string message = "[{started}] - [{finished}] Переобход выполнен за {duration} сек. Найдено {newAlbumsCount} новых альбомов";

            logger.LogInformation(message,
                started,
                LastUpdate,
                (int)sw.Elapsed.TotalSeconds,
                result.Sum(i => i.NewAlbums?.Length));

            NewAlbumsFound?.Invoke(this, result);
        }
        catch (NotSupportedException ex)
        {
            logger.LogWarning($"Метод не поддерживается: {ex.Message}. {ex.StackTrace}");
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка переобхода по таймеру: {error}", ex.Message);
            throw;
        }
        finally
        {
            CrawlerInProgress = false;
        }
    }
    private async Task SaveLastUpdateTime()
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();

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
}
