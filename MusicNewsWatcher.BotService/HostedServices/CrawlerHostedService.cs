using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using MusicNewWatcher.BL;

namespace MusicNewsWatcher.BotService.HostedServices;

public sealed class CrawlerHostedService : BackgroundService
{
    private readonly ILogger<CrawlerHostedService> logger;
    private readonly IMusicNewsMessageFormatter telegramMessageFormatter;
    private readonly MusicNewsWatcherTelegramBot botClient;
    private readonly MusicUpdateManager updateManager;

    public CrawlerHostedService(ILogger<CrawlerHostedService> logger,
        IMusicNewsMessageFormatter telegramMessageFormatter,
        MusicNewsWatcherTelegramBot botClient,
        MusicUpdateManager updateManager)
    {
        this.logger = logger;
        this.telegramMessageFormatter = telegramMessageFormatter;
        this.botClient = botClient;
        this.updateManager = updateManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        updateManager.OnNewAlbumsFound += UpdateManager_OnNewAlbumsFound;

        await updateManager.RefreshIntervalAndLastUpdate();

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Следующий переобход парсера будет запущен через ... {interval} мин.",
                (int)updateManager.UpdateInterval.TotalMinutes);

            await Task.Delay(updateManager.UpdateInterval, stoppingToken);

            try
            {
                logger.LogInformation("Запуск переобхода");

                await updateManager.RunCrawler(stoppingToken);

                var memoryUsageInMb = (int)(GC.GetTotalMemory(forceFullCollection: true) / 1E6);

                logger.LogInformation("Переобход выполнен");
                logger.LogInformation("Занимаемая память приложения: {memoryUsage} мб.", memoryUsageInMb);
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка выполнения переобхода: {error}", ex.Message);
            }
        }

        logger.LogInformation("Выход из службы парсера. Токен отмены: {stoppingToken}", stoppingToken);
    }

    private async void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        try
        {
            var text = telegramMessageFormatter.BuildNewAlbumsFoundMessage(e.Provider, e.Artist!, e.NewAlbums!);
            await botClient.SendTextMessageAsync(text);
        }
        catch
        {
            logger.LogError("Ошибка при нахождении нового альбома");
            throw;
        }
    }

    public override void Dispose()
    {
        updateManager.OnNewAlbumsFound -= UpdateManager_OnNewAlbumsFound;
        base.Dispose();
    }
}

