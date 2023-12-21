using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.TelegramBot;
using MusicNewWatcher.BL;

namespace MusicNewsWatcher.BotService;

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
        logger.LogInformation($"Запуск службы telegram бота ...");

        botClient.OnForceUpdateCommandRecevied += BotRoutes_OnForceUpdateCommandRecevied;
        try
        {
            botClient.Start();
        }
        catch
        {
            throw;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async void BotRoutes_OnForceUpdateCommandRecevied()
    {
        try
        {
            logger.LogInformation("Начало выполнения команды форсированного выполнения");
            await updateManager.CheckUpdatesAllAsync(CancellationToken.None);
            logger.LogInformation("Конец выполнения команды форсированного выполнения");
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

