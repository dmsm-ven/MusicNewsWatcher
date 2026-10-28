using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.Models;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;

namespace MusicNewsWatcher.API.BackgroundServices;

public sealed class CrawlerHostedService(ILogger<CrawlerHostedService> logger,
        MusicWatcherTelegramBotClient telegramBotClient,
        IMusicNewsMessageFormatter telegramBotMessageFormatter,
        MusicUpdateManager updateManager,
        IOptionsMonitor<CrawlerConfiguration> crawlerOptions) : BackgroundService
{
    private const int MaxArtistMessages = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Запуск службы парсера ...");

        await updateManager.RefreshLastUpdateDateTime();
        updateManager.NewAlbumsFound += UpdateManager_NewAlbumsFound;

        try
        {
            // IOptionsMonitor подхватывает изменения appsettings.json без перезапуска (в отличие от IOptions)
            var options = crawlerOptions.CurrentValue;

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
                await Task.Delay(nextExecTime, stoppingToken);
            }

            await RunCrawlerTask(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(crawlerOptions.CurrentValue.CheckInterval, stoppingToken);
                await RunCrawlerTask(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // штатная остановка приложения
        }
        finally
        {
            updateManager.NewAlbumsFound -= UpdateManager_NewAlbumsFound;
        }

        logger.LogInformation("Выход из службы парсера. Токен отмены: {stoppingToken}", stoppingToken);
    }

    // Обработчик события не должен быть async void: необработанное исключение в нём роняет весь процесс
    private void UpdateManager_NewAlbumsFound(object? sender, NewAlbumsFoundEventArgs[] result)
    {
        _ = SendNewAlbumsNotificationsAsync(result);
    }

    private async Task SendNewAlbumsNotificationsAsync(NewAlbumsFoundEventArgs[] result)
    {
        try
        {
            foreach (var data in result.Take(MaxArtistMessages))
            {
                if (data.Artist is null)
                {
                    logger.LogWarning("Null Artist detected for provider {provider}. Skipping message.", data.Provider);
                    continue;
                }

                string messageText = telegramBotMessageFormatter.BuildNewAlbumsFoundMessage(
                    data.Provider,
                    data.Artist,
                    data.NewAlbums
                );
                await telegramBotClient.SendMessage(messageText);
            }

            if (result.Length > MaxArtistMessages)
            {
                var rest = result.Skip(MaxArtistMessages).ToArray();
                string ellipsesMessage = $"И еще {rest.Sum(ar => ar.NewAlbums.Length)} альбомов у {rest.Length} исполнителей";
                await telegramBotClient.SendMessage(ellipsesMessage);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка отправки уведомления о новых альбомах в Telegram");
        }
    }

    private async Task RunCrawlerTask(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogTrace("Запуск переобхода");

            await updateManager.RunCrawler(stoppingToken);

            var memoryUsageInMb = (int)(GC.GetTotalMemory(forceFullCollection: false) / 1E6);

            logger.LogTrace("Занимаемая память приложением: {memoryUsage} мб.", memoryUsageInMb);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка выполнения переобхода");
        }
    }
}
