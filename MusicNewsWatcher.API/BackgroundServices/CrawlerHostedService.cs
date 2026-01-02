using MusicNewsWatcher.API.Services;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class CrawlerHostedService : BackgroundService
{
    public TimeSpan DefaultTimerInterval { get; } = TimeSpan.FromMinutes(60);

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
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(DefaultTimerInterval, stoppingToken);

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

