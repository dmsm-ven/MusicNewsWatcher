using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class MusicWatcherTelegramBotClient
{
    private readonly ITelegramBotClient bot;
    private readonly ILogger<MusicWatcherTelegramBotClient> logger;
    private readonly MusicWatcherTelegramBotConfiguration config;

    //Все обработчики на выполнение команд инициализируем в конструкторе, что бы не усложнять класс и не внедрять DbContext
    private readonly IReadOnlyDictionary<TelegramBotCommand, CommandDescriptorEntry> commandDescriptors;

    private bool isStarted = false;
    public MusicWatcherTelegramBotClient(ITelegramBotClient bot,
        ILogger<MusicWatcherTelegramBotClient> logger,
        MusicWatcherTelegramBotConfiguration config,
        IReadOnlyDictionary<TelegramBotCommand, Func<Task<string>>> commandHandlers)
    {
        this.bot = bot;
        this.logger = logger;
        this.config = config;

        commandDescriptors = new Dictionary<TelegramBotCommand, CommandDescriptorEntry>()
        {
            [TelegramBotCommand.ShowTrackedArtists] = new()
            {
                Route = "/tracked_artists",
                Description = "Получить список отслеживаемых исполнителей",
                Callback = commandHandlers[TelegramBotCommand.ShowTrackedArtists]
            },
            [TelegramBotCommand.ShowLastUpdate] = new()
            {
                Route = "/last_update",
                Description = "Дата последнего парсинга сайтов",
                Callback = commandHandlers[TelegramBotCommand.ShowLastUpdate]
            },
            [TelegramBotCommand.ShowLastParsedAlbums] = new()
            {
                Route = "/last_parsed_albums",
                Description = "Список 10 последних найденных альбомов",
                Callback = commandHandlers[TelegramBotCommand.ShowLastParsedAlbums]
            },
            [TelegramBotCommand.ExecuteForceUpdate] = new()
            {
                Route = "/force_update",
                Description = "Запустить переобход парсера на поиск новых альбомов",
                Callback = commandHandlers[TelegramBotCommand.ExecuteForceUpdate]
            }
        };
    }

    public void Start(CancellationToken stoppingToken)
    {
        if (isStarted)
        {
            throw new InvalidOperationException("Bot already started");
        }

        isStarted = true;

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types };
        };

        bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: stoppingToken);
    }
    public async Task<Message> SendMessage(string text)
    {
        return await bot.SendMessage(config.ClientId, text, ParseMode.Html);
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

        var kvp = commandDescriptors.FirstOrDefault(c => c.Value.Route == update.Message.Text);
        CommandDescriptorEntry descriptor = kvp.Value;
        TelegramBotCommand commandType = kvp.Key;

        if (commandType == TelegramBotCommand.None || descriptor == null)
        {
            await UsageCommand(message);
        }
        //задание с несколькими шагами выполнения
        else if (commandType == TelegramBotCommand.ExecuteForceUpdate)
        {
            await bot.SendMessage(chatId: message.Chat.Id, text: "Переобход запущен", ParseMode.Html);
            string replyText = await descriptor.Callback!();
            await bot.SendMessage(chatId: message.Chat.Id, text: replyText, ParseMode.Html);
        }
        //Для остальных команд просто возращаем сообщение с текстом
        else
        {
            string replyText = await descriptor.Callback!();
            await bot.SendMessage(chatId: message.Chat.Id, text: replyText, ParseMode.Html);
        }
    }
    private Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        logger.LogError(exception, $"Telegram bot error: {exception.Message}");
        return Task.CompletedTask;
    }
    private async Task<Message> UsageCommand(Message message)
    {
        var usageList = new List<string>() {
            "Команды",
            "/start\t - Отобразить список доступных команд"
        };
        foreach (var descriptor in commandDescriptors.Values)
        {
            usageList.Add($"{descriptor.Route}\t - {descriptor.Description}");
        }

        string usageMessage = string.Join(Environment.NewLine, usageList);

        return await bot.SendMessage(chatId: message.Chat.Id,
                                                    text: usageMessage,
                                                    replyMarkup: new ReplyKeyboardRemove());
    }
}
