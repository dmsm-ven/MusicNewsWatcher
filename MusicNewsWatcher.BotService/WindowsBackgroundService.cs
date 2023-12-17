using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;
using MusicNewWatcher.BL;

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
            logger.LogInformation($"Запуск службы ...");

            botRoutes.OnForceUpdateCommandRecevied += BotRoutes_OnForceUpdateCommandRecevied;
            botClient.Start(consumerId);

            updateManager.OnNewAlbumsFound += UpdateManager_OnNewAlbumsFound;

            //убрать await ? иначе не будет прослушиватся остановка, а без await не отлавливает исключения
            await updateManager.Start();

            logger.LogInformation("BackgroundSerivce запущен");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            logger.LogInformation("Выход из программы (получен сигнал на завершение)");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка BckgroundService. Выход ...");
            Environment.Exit(1);
        }
    }

    private async void BotRoutes_OnForceUpdateCommandRecevied()
    {
        try
        {
            logger.LogInformation("Начало выполнения команды форсированного выполнения");
            await updateManager.CheckUpdatesAllAsync();
            logger.LogInformation("Конец выполнения команды форсированного выполнения");
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка выполнения команды форсированного обновления");
        }
    }

    private async void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        try
        {
            logger.LogInformation("Начало отправки сообщения о нахождении нового альбома");

            var text = telegramMessageFormatter.BuildNewAlbumsFoundMessage(e.Provider, e.Artist, e.NewAlbums);
            await botClient.SendTextMessageAsync(text);

            logger.LogInformation("Конец отправки сообщения о нахождении нового альбома");
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка при нахождении нового альбома");
        }
    }

    public override void Dispose()
    {
        if (botRoutes != null)
        {
            botRoutes.OnForceUpdateCommandRecevied -= BotRoutes_OnForceUpdateCommandRecevied;
        }
        if (updateManager != null)
        {
            updateManager.OnNewAlbumsFound -= UpdateManager_OnNewAlbumsFound;
        }

        base.Dispose();
    }
}

