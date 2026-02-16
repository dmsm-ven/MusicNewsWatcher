using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.Models;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class CrawlerHostedService(ILogger<CrawlerHostedService> logger,
        MusicWatcherTelegramBotClient telegramBotClient,
        IMusicNewsMessageFormatter telegramBotMessageFormatter,
        MusicUpdateManager updateManager,
        IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"Запуск службы парсера ...");

        await updateManager.RefreshLastUpdateDateTime();
        updateManager.NewAlbumsFound += UpdateManager_NewAlbumsFound;

        var scope = serviceScopeFactory.CreateScope();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<CrawlerConfiguration>>().Value;

        TimeSpan nextExecTime = options.CheckInterval - (DateTime.UtcNow - updateManager.LastUpdate);
        if (nextExecTime < TimeSpan.Zero)
        {
            nextExecTime = TimeSpan.Zero;
        }

        logger.LogInformation("Интервал обновления парсера: {checkInterval}", options.CheckInterval);
        logger.LogInformation("Последниее обновление было произведено: {dt}", updateManager.LastUpdate);
        logger.LogInformation("Следующее обновление будет выполнено через: {nextExecTime}", nextExecTime);

        if (nextExecTime > TimeSpan.Zero)
        {
            await Task.Delay(nextExecTime);
        }

        await RunCrawlerTask(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            scope = serviceScopeFactory.CreateScope();
            options = scope.ServiceProvider.GetRequiredService<IOptions<CrawlerConfiguration>>().Value;

            await Task.Delay(options.CheckInterval, stoppingToken);
            await RunCrawlerTask(stoppingToken);
        }

        logger.LogInformation("Выход из службы парсера. Токен отмены: {stoppingToken}", stoppingToken);
    }

    private async void UpdateManager_NewAlbumsFound(object? sender, Core.Models.NewAlbumsFoundEventArgs[] result)
    {
        int artistsCounter = 0;
        int maxMessageCount = 5;
        foreach (var data in result)
        {
            if (artistsCounter > maxMessageCount)
            {
                string ellipsesMessage = $"И еще {result.Skip(maxMessageCount).Sum(ar => ar.NewAlbums.Length)} альбомов у {result.Length - maxMessageCount} исполнителей";
                await telegramBotClient.SendMessage(ellipsesMessage);
                break;
            }

            if (data.Artist is not null && data.NewAlbums is not null)
            {
                string messageText = telegramBotMessageFormatter.BuildNewAlbumsFoundMessage(
                    data.Provider,
                    data.Artist,
                    data.NewAlbums
                );
                await telegramBotClient.SendMessage(messageText);
            }
            else
            {
                logger.LogWarning("Null Artist or Albums detected for provider {provider}. Skipping message.", data.Provider);
            }

            artistsCounter++;
        }
    }

    private async Task RunCrawlerTask(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogTrace("Запуск переобхода");

            await updateManager.RunCrawler(stoppingToken);

            var memoryUsageInMb = (int)(GC.GetTotalMemory(forceFullCollection: true) / 1E6);

            logger.LogTrace("Занимаемая память приложением: {memoryUsage} мб.", memoryUsageInMb);
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка выполнения переобхода: {error}", ex.Message);
        }
    }
}

