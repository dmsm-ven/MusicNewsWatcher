using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;

namespace MusicNewsWatcher.API.BackgroundServices;

public class CrawlerConfiguration
{
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(60);
}
public sealed class CrawlerHostedService(ILogger<CrawlerHostedService> logger,
        MusicWatcherTelegramBotClient telegramBotClient,
        IMusicNewsMessageFormatter telegramBotMessageFormatter,
        MusicUpdateManager updateManager,
        IOptionsMonitor<CrawlerConfiguration> crawlerConfiguration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await updateManager.RefreshLastUpdateDateTime();

        updateManager.NewAlbumsFound += UpdateManager_NewAlbumsFound;

        bool needUpdateNow = (DateTime.UtcNow - updateManager.LastUpdate) > crawlerConfiguration.CurrentValue.CheckInterval;

        if (needUpdateNow)
        {
            await RunCrawlerTask(stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(crawlerConfiguration.CurrentValue.CheckInterval, stoppingToken);

            await RunCrawlerTask(stoppingToken);
        }

        logger.LogInformation("Выход из службы парсера. Токен отмены: {stoppingToken}", stoppingToken);
    }

    private async void UpdateManager_NewAlbumsFound(object? sender, Core.Models.NewAlbumsFoundEventArgs[] result)
    {
        foreach (var data in result)
        {
            if (data.Artist is not null && data.NewAlbums is not null)
            {
                string messageText = telegramBotMessageFormatter.BuildNewAlbumsFoundMessage(
                    data.Provider,
                    data.Artist,
                    data.NewAlbums
                );
                await telegramBotClient.SendMessage(messageText);
            }
            else
            {
                logger.LogWarning("Null Artist or Albums detected for provider {provider}. Skipping message.", data.Provider);
            }
        }
    }

    private async Task RunCrawlerTask(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogTrace("Запуск переобхода");

            await updateManager.RunCrawler(stoppingToken);

            var memoryUsageInMb = (int)(GC.GetTotalMemory(forceFullCollection: true) / 1E6);

            logger.LogTrace("Занимаемая память приложением: {memoryUsage} мб.", memoryUsageInMb);
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка выполнения переобхода: {error}", ex.Message);
        }
    }
}

