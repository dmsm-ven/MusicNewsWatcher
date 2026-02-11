using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class MusicWatcherTelegramBotClient
{
    private readonly MusicWatcherTelegramBotConfiguration config;
    private readonly ILogger<MusicWatcherTelegramBotClient> logger;
    private readonly ITelegramBotClient bot;
    private readonly Dictionary<TelegramBotCommand, Func<string>> commandHandlers;
    private bool isStarted = false;

    public MusicWatcherTelegramBotClient(ITelegramBotClient bot,
        ILogger<MusicWatcherTelegramBotClient> logger,
        IOptions<MusicWatcherTelegramBotConfiguration> options)
    {
        this.bot = bot;
        this.logger = logger;
        this.config = options?.Value ?? throw new InvalidOperationException("TG bot options not provided");
        this.commandHandlers = new Dictionary<TelegramBotCommand, Func<string>>();
    }

    public async Task Start(CancellationToken stoppingToken, Dictionary<TelegramBotCommand, Func<string>> commandHandlers)
    {
        if (isStarted)
        {
            throw new InvalidOperationException("Bot already started");
        }

        isStarted = true;

        this.commandHandlers[TelegramBotCommand.None] = () => "Функционал отключен";
        foreach (var k in commandHandlers.Where(h => h.Value != null))
        {
            this.commandHandlers[k.Key] = k.Value;
        }

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types };
        };

        bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: stoppingToken);

    }

    public async Task<Message> SendMessage(string text)
    {
        return await bot.SendMessage(config.ClientId, text);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
    {
        var sender = update.Message?.From;

        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        if (string.IsNullOrWhiteSpace(message?.Text))
            return;

        logger.LogInformation("Получена команда в TG бот: {message} от {sender} ({firstName} {lastName})",
            message.Text,
            sender?.Username ?? "",
            sender?.FirstName ?? "",
            sender?.LastName ?? "");


        switch (update.Message.Text)
        {
            case "/last_update":
                await LastUpdateCommand(message);
                break;
            case "/force_update":
                await ForceUpdateCommand(message);
                break;
            case "/tracked_artists":
                await ProviderListCommand(message);
                break;
            default:
                await UsageCommand(message);
                break;
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        logger.LogError(exception, "Telegram bot error");
        return Task.CompletedTask;
    }

    private async Task<Message> UsageCommand(Message message)
    {
        var usageList = new List<string>() {
            "Команды:",
            "/tracked_artists\t - Получить список отслеживаемых исполнителей",
            "/last_update\t - дата последнего парсинга сайтов",
            "/force_update\t - проверить новинки сейчас"
        };

        string usageMessage = string.Join(Environment.NewLine, usageList);

        return await bot.SendMessage(chatId: message.Chat.Id,
                                                    text: usageMessage,
                                                    replyMarkup: new ReplyKeyboardRemove());
    }

    private async Task<Message> ForceUpdateCommand(Message message)
    {
        commandHandlers.TryGetValue(TelegramBotCommand.ExecutForceUpdate, out var callback);
        string replyText = callback?.Invoke() ?? commandHandlers[TelegramBotCommand.None].Invoke();

        return await bot.SendMessage(chatId: message.Chat.Id,
                                text: replyText);
    }

    private async Task<Message> LastUpdateCommand(Message message)
    {
        commandHandlers.TryGetValue(TelegramBotCommand.ShowLastUpdate, out var callback);
        string replyText = callback?.Invoke() ?? commandHandlers[TelegramBotCommand.None].Invoke();
        return await bot.SendMessage(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);

    }

    private async Task<Message> ProviderListCommand(Message message)
    {
        commandHandlers.TryGetValue(TelegramBotCommand.ShowTrackedArtists, out var callback);
        string replyText = callback?.Invoke() ?? commandHandlers[TelegramBotCommand.None].Invoke();

        return await bot.SendMessage(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);
    }
}

public enum TelegramBotCommand
{
    None,
    ShowLastUpdate,
    ExecutForceUpdate,
    ShowTrackedArtists
}

