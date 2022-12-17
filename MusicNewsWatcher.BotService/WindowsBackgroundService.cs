using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;
using Telegram.Bot;

public sealed class ServiceBackgroundWorker : BackgroundService
{
    private readonly MusicNewsWatcherTelegramBot botClient;
    private readonly TelegramBotRoutes botRoutes;
    private readonly MusicUpdateManager updateManager;
    private readonly IConfiguration configuration;
    private readonly IMusicNewsMessageFormatter telegramMessageFormatter;
    private readonly ILogger<ServiceBackgroundWorker> logger;

    public ServiceBackgroundWorker(MusicNewsWatcherTelegramBot botClient,
        TelegramBotRoutes botRoutes,
        MusicUpdateManager updateManager,
        IConfiguration configuration,
        IMusicNewsMessageFormatter telegramMessageFormatter,
        ILogger<ServiceBackgroundWorker> logger)
    {
        this.botClient = botClient;
        this.botRoutes = botRoutes;
        this.updateManager = updateManager;
        this.configuration = configuration;
        this.telegramMessageFormatter = telegramMessageFormatter;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            string consumerId = configuration["TelegramBot:ClientId"];
            logger.LogInformation($"Запуск BackgroundSerivce (consumer_id: {consumerId}) ...");

            botRoutes.OnForceUpdateCommandRecevied += async () =>
            {
                await updateManager.CheckUpdatesAllAsync();
            };
            botClient.Start(consumerId);

            updateManager.OnNewAlbumsFound += async (o, e) =>
            {
                var text = telegramMessageFormatter.BuildNewAlbumsFoundMessage(e.Provider, e.Artist, e.NewAlbums);
                await botClient.SendTextMessageAsync(text);
            };
            updateManager.Start();

            logger.LogInformation("BackgroundSerivce запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка BckgroundService. Выход ...");
            Environment.Exit(1);
        }
    }
}

