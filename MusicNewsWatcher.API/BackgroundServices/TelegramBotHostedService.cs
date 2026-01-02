using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class TelegramBotHostedService : BackgroundService
{
    private readonly MusicWatcherTelegramBotClient bot;
    private readonly MusicUpdateManager updateManager;
    private readonly ILogger<TelegramBotHostedService> logger;

    public TelegramBotHostedService(MusicWatcherTelegramBotClient bot,
        MusicUpdateManager updateManager,
        ILogger<TelegramBotHostedService> logger)
    {
        this.bot = bot;
        this.updateManager = updateManager;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Запуск службы Telegram бота ...");

        try
        {
            bot.Start(stoppingToken);
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
}

