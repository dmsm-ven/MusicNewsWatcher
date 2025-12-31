using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class TelegramBotHostedService : BackgroundService
{
    private readonly MusicNewsWatcherTelegramBot botClient;
    private readonly MusicUpdateManager updateManager;
    private readonly ILogger<TelegramBotHostedService> logger;

    public TelegramBotHostedService(MusicNewsWatcherTelegramBot botClient,
        MusicUpdateManager updateManager,
        ILogger<TelegramBotHostedService> logger)
    {
        this.botClient = botClient;
        this.updateManager = updateManager;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Запуск службы Telegram бота ...");

        botClient.OnForceUpdateCommandRecevied += BotRoutes_OnForceUpdateCommandRecevied;
        try
        {
            botClient.Start();
            logger.LogInformation("Telegram бот запущен");
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка при выполнении Telegram бота {message}", ex.Message);
            throw;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        logger.LogInformation("Выход из службы телеграм бота. Токен отмены: {stoppingToken}", stoppingToken);
    }

    private async void BotRoutes_OnForceUpdateCommandRecevied()
    {
        try
        {
            logger.LogTrace("Начало выполнения команды форсированного выполнения");
            var newAlbumsFound = await updateManager.CheckUpdatesAllAsync(CancellationToken.None);
            logger.LogTrace("Конец выполнения команды форсированного выполнения. Найдено {newAlbumsFound} новых альбомов",
                newAlbumsFound);
        }
        catch
        {
            logger.LogError("Ошибка выполнения команды форсированного обновления");
            throw;
        }
    }

    public override void Dispose()
    {
        botClient?.Dispose();
        base.Dispose();
    }
}

