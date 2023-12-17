using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace MusicNewsWatcher.TelegramBot;

public class MusicNewsWatcherTelegramBot : IDisposable
{
    private readonly TelegramBotRoutes thBotMessageHandler;
    public bool IsStarted { get; private set; }

    private readonly ITelegramBotClient botClient;
    private readonly TelegramBotRoutes botRoutes;
    private readonly ILogger<MusicNewsWatcherTelegramBot> logger;

    private readonly CancellationTokenSource cts = new();
    private string? consumerId;

    public MusicNewsWatcherTelegramBot(ITelegramBotClient botClient,
        TelegramBotRoutes? botRoutes,
        ILogger<MusicNewsWatcherTelegramBot> logger)
    {
        this.botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        this.botRoutes = botRoutes ?? throw new ArgumentNullException(nameof(botRoutes));
        this.logger = logger;
    }

    public void Start(string consumerId)
    {
        this.consumerId = consumerId;

        if (IsStarted)
        {
            logger.LogError("Бот был уже запущен");
            throw new NotSupportedException();
        }

        if (string.IsNullOrWhiteSpace(consumerId))
        {
            logger.LogError("Telegram consumerId not provided");
            throw new ArgumentNullException(nameof(consumerId), "Telegram consumerId not provided");
        }

        logger.LogInformation("Запуск бота ...");

        botClient.StartReceiving(botRoutes);

        IsStarted = true;

        logger.LogInformation("Бот запущен");
    }

    public async Task SendTextMessageAsync(string text)
    {
        await botClient.SendTextMessageAsync(consumerId, text, Telegram.Bot.Types.Enums.ParseMode.Html);

        logger.LogInformation("Отправка сообщения");
    }

    public void Stop()
    {
        cts.Cancel();

        logger.LogInformation("Бот остановлен");
    }

    public void Dispose()
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}