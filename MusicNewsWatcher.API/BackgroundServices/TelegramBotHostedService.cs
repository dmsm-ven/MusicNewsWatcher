using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class TelegramBotHostedService(MusicWatcherTelegramBotClient telegramBotClient,
        MusicUpdateManager updateManager,
        ILogger<TelegramBotHostedService> logger) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Запуск службы Telegram бота ...");

        try
        {
            telegramBotClient.Start(stoppingToken);
            await telegramBotClient.SendMessage($"Бот по парсингу запущен в {DateTime.UtcNow.ToRussianLocalTime()}");
            logger.LogInformation("Telegram bot запущен");

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