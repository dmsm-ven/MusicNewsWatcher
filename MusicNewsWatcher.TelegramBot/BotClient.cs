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
            logger.LogWarning("Попытка запустить бот когда он уже запущен ... отмена");
            return;
        }

        if (string.IsNullOrWhiteSpace(consumerId))
        {
            logger.LogError("ConsumerId не был предоставлен");
            throw new ArgumentNullException(nameof(consumerId), "onsumerId не был предоставлен");
        }

        botClient.StartReceiving(botRoutes);

        IsStarted = true;
    }

    public async Task SendTextMessageAsync(string text)
    {
        await botClient.SendTextMessageAsync(consumerId, text, Telegram.Bot.Types.Enums.ParseMode.Html);

        logger.LogInformation("Отправка сообщения пользователю с ID {consumerId}", consumerId);
    }

    public void Stop()
    {
        cts.Cancel();

        logger.LogInformation("Бот остановлен");
    }

    public void Dispose()
    {
        cts?.Dispose();
    }
}