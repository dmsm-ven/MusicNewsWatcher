using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class CrawlerHostedService(ILogger<CrawlerHostedService> logger,
        MusicWatcherTelegramBotClient telegramBotClient,
        IMusicNewsMessageFormatter telegramBotMessageFormatter,
        MusicUpdateManager updateManager) : BackgroundService
{
    public TimeSpan DefaultTimerInterval { get; } = TimeSpan.FromMinutes(60);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        updateManager.NewAlbumsFound += UpdateManager_NewAlbumsFound;
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(DefaultTimerInterval, stoppingToken);

            await RunCrawlerTask(stoppingToken);
        }

        logger.LogInformation("Выход из службы парсера. Токен отмены: {stoppingToken}", stoppingToken);
    }

    private async void UpdateManager_NewAlbumsFound(object? sender, Core.Models.NewAlbumsFoundEventArgs[] result)
    {
        foreach (var data in result)
        {
            string messageText = telegramBotMessageFormatter.BuildNewAlbumsFoundMessage(data.Provider, data.Artist, data.NewAlbums);
            await telegramBotClient.SendMessage(messageText);
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

