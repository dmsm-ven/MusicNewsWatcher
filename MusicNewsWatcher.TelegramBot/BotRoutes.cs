using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;

namespace MusicNewsWatcher.TelegramBot;

public class TelegramBotRoutes : IUpdateHandler
{
    public event Action OnForceUpdateCommandRecevied;

    private readonly TelegramBotCommandHandlers commandHandlers;
    private readonly ITelegramBotClient botClient;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContextFactory;
    private readonly ILogger<TelegramBotRoutes> logger;

    public TelegramBotRoutes(TelegramBotCommandHandlers commandHandlers,
        ILogger<TelegramBotRoutes> looger)
    {
        this.commandHandlers = commandHandlers ?? throw new ArgumentNullException(nameof(commandHandlers));
        this.logger = looger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var handler = update.Type switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            //UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
            //UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
            //UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery!),
            //UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult!),

            UpdateType.Message => BotOnMessageReceived(update.Message!),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update),
            _ => throw new NotImplementedException()
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TelegramBot ERROR: {ex.Message}");
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Ошибка роутинга");
        return Task.CompletedTask;
    }

    private Task BotOnMessageReceived(Message message)
    {
        logger.LogInformation("Получена команда: " + message?.Text ?? "<Пусто>");

        if (string.IsNullOrWhiteSpace(message?.Text))
            return Task.CompletedTask;

        Dictionary<string, Func<Task<Message?>>> routes = new();
        routes["/last_update"] = () => commandHandlers.LastUpdate(message);
        routes["/force_update"] = () => commandHandlers.ForceUpdate(message);
        routes["/tracked_artists"] = () => commandHandlers.ProviderList(message);

        if (routes.ContainsKey(message.Text))
        {
            if (message.Text == "/force_update")
            {
                OnForceUpdateCommandRecevied?.Invoke();
            }
            return routes[message.Text]();
        }

        return commandHandlers.Usage(message);
    }

    private Task<Message> BotOnCallbackQueryReceived(Update update)
    {
        var text = update.CallbackQuery.Data;
        var param = text.Substring(text.IndexOf(" ") + 1);

        if (text.StartsWith("/tracked_artists_for_provider"))
        {
            return commandHandlers.TrackedArtistsForProvider(update, param);
        }

        throw new NotImplementedException();
    }
}
