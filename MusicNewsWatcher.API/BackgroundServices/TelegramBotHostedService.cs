using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class TelegramBotHostedService(MusicWatcherTelegramBotClient telegramBotClient,
        MusicUpdateManager updateManager,
        IDbContextFactory<MusicWatcherDbContext> dbFactory,
        ILogger<TelegramBotHostedService> logger) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Запуск службы Telegram бота ...");
        var handlers = new Dictionary<TelegramBotCommand, Func<string>>()
        {
            [TelegramBotCommand.ExecutForceUpdate] = () =>
            {
                if (!updateManager.CrawlerInProgress)
                {
                    updateManager.RunCrawler(CancellationToken.None);
                    return "Переообход запущен";
                }
                return "Переообход уже в процессе выполнения";
            },
            [TelegramBotCommand.ShowLastUpdate] = () =>
            {
                return $"Последний перееобход был {updateManager.LastUpdate.ToLocalTime()}";
            },
            [TelegramBotCommand.ShowTrackedArtists] = () =>
            {
                using var db = dbFactory.CreateDbContext();
                var lines = db.MusicProviders
                    .AsNoTracking()
                    .Select(p => p.Name)
                    .ToArray()
                    .Select((providerName, index) => $"{index + 1}. {providerName}");
                string message = string.Join("\r\n", lines);
                return message;
            }
        };

        try
        {
            telegramBotClient.Start(stoppingToken, handlers);

            DateTime convertedTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
            await telegramBotClient.SendMessage($"Бот по парсингу запущен в {convertedTime.ToString()}");
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