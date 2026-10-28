using MusicNewsWatcher.TelegramBot;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class TelegramBotHostedService(MusicWatcherTelegramBotClient telegramBotClient,
        ILogger<TelegramBotHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Запуск службы Telegram бота ...");

        try
        {
            // Проверка связи занимает не больше 5 секунд. Если Telegram недоступен, бот отключается (IsEnabled = false)
            await telegramBotClient.StartAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при запуске Telegram бота. Telegram функционал отключён");
            return;
        }

        if (!telegramBotClient.IsEnabled)
        {
            logger.LogWarning("Telegram бот отключён, служба завершает работу. Остальные функции API работают в обычном режиме");
            return;
        }

        logger.LogInformation("Telegram bot запущен");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // штатная остановка приложения
        }

        logger.LogInformation("Выход из службы телеграм бота. Токен отмены: {stoppingToken}", stoppingToken);
    }
}
