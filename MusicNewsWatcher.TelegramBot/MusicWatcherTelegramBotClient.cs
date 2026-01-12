using Microsoft.Extensions.Logging;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class MusicWatcherTelegramBotClient
{
    private readonly MusicWatcherTelegramBotConfiguration options;
    private readonly IMusicNewsMessageFormatter messageFormatter;
    private readonly ILogger<MusicWatcherTelegramBotClient> logger;
    private readonly ITelegramBotClient bot;

    public MusicWatcherTelegramBotClient(ITelegramBotClient bot,
        IMusicNewsMessageFormatter messageFormatter,
        ILogger<MusicWatcherTelegramBotClient> logger)
    {
        this.bot = bot;
        this.messageFormatter = messageFormatter;
        this.logger = logger;
    }

    public void Start(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types };
        };

        bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: stoppingToken);
    }

    public async Task<Message> NotifyAboutNewAlbumsFound(long chatId, string text)
    {
        return await bot.SendMessage(chatId, text);
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
        return await bot.SendMessage(chatId: message.Chat.Id,
                                text: "Функционал отключен");
    }

    private async Task<Message> LastUpdateCommand(Message message)
    {
        string replyText = $"Функционал отключен";
        return await bot.SendMessage(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);

    }

    private async Task<Message> ProviderListCommand(Message message)
    {
        string replyText = $"Функционал отключен";
        return await bot.SendMessage(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);
    }

}

