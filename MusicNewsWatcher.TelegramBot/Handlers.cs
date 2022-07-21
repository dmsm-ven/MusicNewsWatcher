using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

internal static class UpdateHandlers
{
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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

            UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
            _ => UnknownUpdateHandlerAsync(botClient, update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await PollingErrorHandler(botClient, exception, cancellationToken);
        }
    }

    private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
    {
        Console.WriteLine($"Receive message type: {message.Type}");
        if (message.Text is not { } messageText)
            return;

        string textCommand = messageText.Split(' ')[0];
        string? textCommandParams = messageText.IndexOf(' ') == -1 ? null : messageText.Substring(messageText.IndexOf(' ') + 1);

        Dictionary<string, Func<Task<Message?>>> routes = new();
        routes["/tracked_artists"] = () => UpdateBotHandlers.TrackedArtists(botClient, message, textCommandParams);

        Task<Message?> routedHandler;

        if (routes.ContainsKey(textCommand))
        {
            routedHandler = routes[textCommand].Invoke();
        }
        else
        {
            routedHandler = Usage(botClient, message);
        }
        
        Message sentMessage = await routedHandler;

        Console.WriteLine($"The message was sent '{messageText}' with id: {sentMessage.MessageId}");
    }
  
    private static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
    {
        var usageList = new List<string>() { 
            "Команды:",
            "/tracked_artists   - Получить список отслеживаемых исполнителей"
        };

        string usageMessage = string.Join(Environment.NewLine, usageList);

        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: usageMessage,
                                                    replyMarkup: new ReplyKeyboardRemove());
    }

    public static Task PollingErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
    {
        Console.WriteLine($"Unknown update type: {update.Type}");
        return Task.CompletedTask;
    }

}

internal static class UpdateBotHandlers
{
    public static IReadOnlyDictionary<string, List<string>> TestData { get; }

    public static async Task<Message> TrackedArtists(ITelegramBotClient botClient, Message message, string? userPickedProvider = null)
    {
        using var db = await MusicNewsWatcherTelegramBot.DbFactory.CreateDbContextAsync();

        if (userPickedProvider == null)
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] 
            {
                db.MusicProviders.Select(pr => new KeyboardButton($"/tracked_artists {pr.Name}" )).ToArray()
            });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Для какого провайдера ?",
                                                        replyMarkup: replyKeyboardMarkup);
        }
        else
        {
            var dbProvider = db.MusicProviders.Include("Artists").FirstOrDefault(pr => pr.Name == userPickedProvider);

            if (dbProvider != null)
            {
                string[] artistLines = dbProvider.Artists
                    .OrderBy(a => a.Name)
                    .Select((artist, i) => $"[{i + 1}] {artist.Name}")
                    .ToArray();
                string replyText = string.Join("\r\n", artistLines);

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                text: replyText,
                                                replyMarkup: new ReplyKeyboardRemove());
            }
            else
            {
                return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Такого провайдера нет");
            }
        }

    }
}
