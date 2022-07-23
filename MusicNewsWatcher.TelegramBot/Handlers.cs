using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core;
using System.Globalization;
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
    public static IDbContextFactory<MusicWatcherDbContext> DbContextFactory { get; set; }

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
        routes["/tracked_artists"] = () => BotCommandHandlers.TrackedArtists(botClient, message, textCommandParams);
        routes["/last_update"] = () => BotCommandHandlers.LastUpdate(botClient, message);

        Task<Message?> routedHandler = routes.ContainsKey(textCommand) ? 
            routes[textCommand].Invoke() : 
            BotCommandHandlers.Usage(botClient, message);
        Message sentMessage = await routedHandler;

        Console.WriteLine($"received from client: '{textCommand}'");
        Console.WriteLine($"bot answer: '{(sentMessage?.Text ?? "<not a text>")}'");
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

internal static class BotCommandHandlers
{
    public static IDbContextFactory<MusicWatcherDbContext> dbFactory { get; set; }

    public static async Task<Message> LastUpdate(ITelegramBotClient botClient, Message message)
    {
        const int textProgressBarLength = 30;

        using var db = await dbFactory.CreateDbContextAsync();

        string lastUpdateString = db.Settings.Find("LastFullUpdateDateTime")?.Value ?? "01.01.2000 00:00:00";
        DateTime lastUpdate = DateTime.Parse(lastUpdateString, new CultureInfo("ru-RU"));
        TimeSpan updateInterval = TimeSpan.FromMinutes(int.Parse(db.Settings.Find("UpdateManagerIntervalInMinutes")?.Value ?? "0"));
        int updateIntervalMinutes = (int)updateInterval.TotalMinutes;
        DateTime nextUpdate = lastUpdate.AddMinutes(updateIntervalMinutes);

        var sb = new StringBuilder()
            .AppendLine($"Дата/время последнего обновления: <b>{lastUpdate.ToString("dd.MM.yyyy HH:mm")}</b>")
            .AppendLine($"Интервал обновления: <b>{updateIntervalMinutes} мин.</b>")
            .Append($"Следующий запуск: <b>{nextUpdate.ToString("dd.MM.yyyy HH:mm")}</b>");


        string replyText = sb.ToString();
        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);
        
    }
    
    public static async Task<Message> TrackedArtists(ITelegramBotClient botClient, Message message, string? userPickedProvider = null)
    {
        using var db = await dbFactory.CreateDbContextAsync();

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

    public static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
    {
        var usageList = new List<string>() {
            "Команды:",
            "/tracked_artists\t - Получить список отслеживаемых исполнителей",
            "/last_update\t - дата последнего парсинга сайтов"
        };

        string usageMessage = string.Join(Environment.NewLine, usageList);

        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: usageMessage,
                                                    replyMarkup: new ReplyKeyboardRemove());
    }
}
