using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace MusicNewsWatcher.TelegramBot;

public sealed class MusicNewsWatcherTelegramBot : IDisposable
{
    public bool IsStarted { get; private set; }

    public event Action? OnForceUpdateCommandRecevied;

    private readonly ITelegramBotClient botClient;
    private readonly TelegramBotRoutes botRoutes;
    private readonly ILogger<MusicNewsWatcherTelegramBot> logger;

    private readonly CancellationTokenSource cts = new();
    private readonly string consumerId;

    public MusicNewsWatcherTelegramBot(ITelegramBotClient botClient,
        TelegramBotRoutes? botRoutes,
        ILogger<MusicNewsWatcherTelegramBot> logger,
        IOptions<TelegramBotConfiguration> telegramBotConfigurationOptions)
    {
        this.botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        this.botRoutes = botRoutes ?? throw new ArgumentNullException(nameof(botRoutes));
        this.consumerId = telegramBotConfigurationOptions?.Value?.ClientId ?? throw new ArgumentNullException(nameof(consumerId));
        this.logger = logger;

        this.botRoutes.OnForceUpdateCommandRecevied += () => OnForceUpdateCommandRecevied?.Invoke();
    }

    public void Start()
    {
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