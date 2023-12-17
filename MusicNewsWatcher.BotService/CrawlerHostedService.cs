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
        await updateManager.Start();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
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
}

