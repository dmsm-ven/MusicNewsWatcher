using MusicNewsWatcher.API.Services;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class CrawlerHostedService : BackgroundService
{
    private readonly ILogger<CrawlerHostedService> logger;
    private readonly MusicUpdateManager updateManager;

    public CrawlerHostedService(ILogger<CrawlerHostedService> logger,
        MusicUpdateManager updateManager)
    {
        this.logger = logger;
        this.updateManager = updateManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await updateManager.RefreshIntervalAndLastUpdate();

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Следующий переобход парсера будет запущен [{nextExecuteDt}] (через {interval} мин.)",
                DateTimeOffset.UtcNow.Add(updateManager.UpdateInterval),
                (int)updateManager.UpdateInterval.TotalMinutes);

            await Task.Delay(updateManager.UpdateInterval, stoppingToken);

            await RunCrawlerTask(stoppingToken);
        }

        logger.LogInformation("Выход из службы парсера. Токен отмены: {stoppingToken}", stoppingToken);
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

