using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using MusicNewWatcher.BL;

namespace MusicNewsWatcher.BotService;

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

        TimeSpan startDelay = TimeSpan.FromSeconds(5);

        await updateManager.RefreshInterval();

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Следующий переобход парсера будет запущен через ... {interval}", updateManager.UpdateInterval);

            await Task.Delay(updateManager.UpdateInterval, stoppingToken);
            try
            {
                logger.LogInformation("[{now}] Запуск переобхода", DateTime.Now);
                await updateManager.RunCrawler(stoppingToken);
                logger.LogInformation("[{now}] Переобход выполнен", DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.LogError("[{now}] Ошибка выполнения переобхода: {error}", DateTime.Now, ex.Message);
            }
        }
    }

    private async void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        try
        {
            logger.LogInformation("Начало отправки сообщения о нахождении нового альбома");

            var text = telegramMessageFormatter.BuildNewAlbumsFoundMessage(e.Provider, e.Artist!, e.NewAlbums!);
            await botClient.SendTextMessageAsync(text);

            logger.LogInformation("Конец отправки сообщения о нахождении нового альбома");
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

