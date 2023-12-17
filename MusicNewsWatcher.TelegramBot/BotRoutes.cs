using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MusicNewsWatcher.TelegramBot;

public class TelegramBotRoutes : IUpdateHandler
{
    public event Action? OnForceUpdateCommandRecevied;

    private readonly TelegramBotCommandHandlers commandHandlers;
    //private readonly IDbContextFactory<MusicWatcherDbContext> dbContextFactory;
    private readonly ILogger<TelegramBotRoutes> logger;

    public TelegramBotRoutes(
        TelegramBotCommandHandlers commandHandlers,
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
        logger.LogInformation("Получена команда для телеграм бота: {message}", message?.Text);

        if (string.IsNullOrWhiteSpace(message?.Text))
            return Task.CompletedTask;

        Dictionary<string, Func<Task<Message?>>> routes = new()
        {
            ["/last_update"] = () => commandHandlers.LastUpdateCommand(message)!,
            ["/force_update"] = () => commandHandlers.ForceUpdateCommand(message)!,
            ["/tracked_artists"] = () => commandHandlers.ProviderListCommand(message)!
        };

        if (routes.ContainsKey(message.Text))
        {
            if (message.Text == "/force_update")
            {
                OnForceUpdateCommandRecevied?.Invoke();
            }
            return routes[message.Text]();
        }

        return commandHandlers.UsageCommand(message);
    }

    private Task<Message> BotOnCallbackQueryReceived(Update update)
    {
        if (!string.IsNullOrWhiteSpace(update?.CallbackQuery?.Data))
        {
            string text = update.CallbackQuery.Data;
            string param = text.Substring(text.IndexOf(" ") + 1);

            if (text.StartsWith("/tracked_artists_for_provider"))
            {
                return commandHandlers.TrackedArtistsForProviderCommand(update, param);
            }
        }
        return Task.FromResult(new Message());
    }
}
