using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MusicNewsWatcher.TelegramBot;

public class MusicNewsWatcherTelegramBot : IDisposable
{
    private TelegramBotRoutes thBotMessageHandler;
    public bool IsStarted { get; private set; }

    public event Action OnForceUpdateCommandReceved
    {
        add
        {
            thBotMessageHandler.OnForceUpdateCommandRecevied += value;
        }
        remove
        {
            thBotMessageHandler.OnForceUpdateCommandRecevied -= value;
        }
    }

    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly ILogger<MusicNewsWatcherTelegramBot> logger;

    private readonly CancellationTokenSource cts;
    private readonly string? apiToken;
    private readonly string? consumerId;

    private TelegramBotClient botClient;

    public MusicNewsWatcherTelegramBot(
        IDbContextFactory<MusicWatcherDbContext> dbFactory,
        ILogger<MusicNewsWatcherTelegramBot> logger)
    {
        this.dbFactory = dbFactory;
        this.logger = logger;
        cts = new CancellationTokenSource();
    }

    public async Task Start(string apiToken, string consumerId)
    {
        if (IsStarted)
        {
            throw new NotSupportedException();
        }

        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentNullException(nameof(apiToken), "Telegram api token not provided");
        }

        if (string.IsNullOrWhiteSpace(consumerId))
        {
            throw new ArgumentNullException(nameof(consumerId), "Telegram consumerId not provided");
        }

        logger.LogInformation("Запуск бота");

        botClient = new TelegramBotClient(apiToken);
        thBotMessageHandler = new TelegramBotRoutes(botClient, dbFactory);
        botClient.StartReceiving(thBotMessageHandler);

        IsStarted = true;
        logger.LogInformation("Бот запущен!");
    }

    public async Task SendTextMessageAsync(string text)
    {
        await botClient.SendTextMessageAsync(consumerId, text, Telegram.Bot.Types.Enums.ParseMode.Html);
    }

    public void Stop()
    {
        cts.Cancel();
    }

    public void Dispose()
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}