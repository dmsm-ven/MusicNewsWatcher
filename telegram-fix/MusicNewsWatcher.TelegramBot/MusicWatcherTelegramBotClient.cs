using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class MusicWatcherTelegramBotClient
{
    /// <summary>Сколько ждём ответа Telegram при старте. Если не успели - функционал бота отключается</summary>
    private static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(5);

    /// <summary>Максимальное время отправки одного сообщения (иначе недоступный Telegram держал бы вызывающий код ~100 сек)</summary>
    private static readonly TimeSpan SendTimeout = TimeSpan.FromSeconds(15);

    private static readonly TimeSpan MaxPollingBackoff = TimeSpan.FromSeconds(60);
    private static readonly TimeSpan ErrorLogInterval = TimeSpan.FromMinutes(1);

    private readonly ITelegramBotClient bot;
    private readonly ILogger<MusicWatcherTelegramBotClient> logger;
    private readonly MusicWatcherTelegramBotConfiguration config;

    //Все обработчики на выполнение команд инициализируем в конструкторе, что бы не усложнять класс и не внедрять DbContext
    private readonly IReadOnlyDictionary<TelegramBotCommand, CommandDescriptorEntry> commandDescriptors;

    private bool isStarted = false;

    // Состояние для ограничения частоты ошибок опроса (обработчик ошибок вызывается из одного цикла опроса)
    private int consecutivePollingErrors;
    private DateTime lastPollingErrorUtc = DateTime.MinValue;
    private DateTime lastPollingErrorLoggedUtc = DateTime.MinValue;

    /// <summary>
    /// true - связь с Telegram установлена и бот работает.
    /// false - бот выключен (отключён в настройках или Telegram недоступен при старте): сообщения не отправляются.
    /// </summary>
    public bool IsEnabled { get; private set; }

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

    /// <summary>
    /// Проверяет связь с Telegram (не дольше ConnectTimeout) и только после этого запускает приём команд.
    /// Если связи нет - бот отключается до перезапуска приложения, об этом пишется одно предупреждение.
    /// </summary>
    public async Task StartAsync(CancellationToken stoppingToken)
    {
        if (isStarted)
        {
            throw new InvalidOperationException("Bot already started");
        }

        isStarted = true;

        if (!config.Enabled)
        {
            logger.LogInformation("Telegram бот отключён в настройках (Enabled = false)");
            return;
        }

        using var connectCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        connectCts.CancelAfter(ConnectTimeout);

        try
        {
            var me = await bot.GetMe(connectCts.Token);
            logger.LogInformation("Telegram бот подключён: @{username}", me.Username);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            return;
        }
        catch (Exception ex)
        {
            string reason = connectCts.IsCancellationRequested
                ? "нет ответа в отведённое время"
                : ex.GetBaseException().Message;

            logger.LogWarning("Не удалось подключиться к Telegram за {timeout} сек. Уведомления и команды Telegram отключены до перезапуска приложения. " +
                "Проверьте доступность Telegram и настройки прокси (ProxySettings). Причина: {reason}",
                (int)ConnectTimeout.TotalSeconds,
                reason);
            return;
        }

        IsEnabled = true;

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types };
        };

        bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: stoppingToken);
    }

    /// <summary>
    /// Отправляет сообщение. Если бот отключён - ничего не делает и возвращает null
    /// </summary>
    public async Task<Message?> SendMessage(string text)
    {
        if (!IsEnabled)
        {
            logger.LogDebug("Telegram бот отключён, сообщение не отправлено");
            return null;
        }

        using var sendCts = new CancellationTokenSource(SendTimeout);
        return await bot.SendMessage(config.ClientId, text, ParseMode.Html, cancellationToken: sendCts.Token);
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

        // Раз обновление получено, связь с Telegram работает - сбрасываем счётчик ошибок опроса
        consecutivePollingErrors = 0;

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

    /// <summary>
    /// Библиотека сразу повторяет запрос после ошибки опроса и ждёт окончания этого обработчика.
    /// Поэтому здесь задаётся нарастающая пауза (2, 4, 8 ... до 60 сек) и ограничивается частота записей в лог:
    /// иначе при недоступном Telegram ошибка повторялась бы много раз в секунду.
    /// </summary>
    private async Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
    {
        var now = DateTime.UtcNow;

        // Если ошибок давно не было, серия считается новой
        if (now - lastPollingErrorUtc > TimeSpan.FromMinutes(5))
        {
            consecutivePollingErrors = 0;
        }
        lastPollingErrorUtc = now;
        consecutivePollingErrors++;

        if (consecutivePollingErrors == 1 || now - lastPollingErrorLoggedUtc >= ErrorLogInterval)
        {
            lastPollingErrorLoggedUtc = now;
            logger.LogError("Telegram bot error: {message}. Повторные попытки выполняются с нарастающей паузой (ошибок подряд: {errors})",
                exception.GetBaseException().Message,
                consecutivePollingErrors);
        }

        var delay = TimeSpan.FromSeconds(Math.Pow(2, Math.Min(consecutivePollingErrors, 6)));
        if (delay > MaxPollingBackoff)
        {
            delay = MaxPollingBackoff;
        }

        try
        {
            await Task.Delay(delay, token);
        }
        catch (OperationCanceledException)
        {
            // приложение останавливается
        }
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
