using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class TelegramBotCommandHandlers(ITelegramBotClient botClient)
{
    public async Task<Message> UsageCommand(Message message)
    {
        var usageList = new List<string>() {
            "Команды:",
            "/tracked_artists\t - Получить список отслеживаемых исполнителей",
            "/last_update\t - дата последнего парсинга сайтов",
            "/force_update\t - проверить новинки сейчас"
        };

        string usageMessage = string.Join(Environment.NewLine, usageList);

        return await botClient.SendMessage(chatId: message.Chat.Id,
                                                    text: usageMessage,
                                                    replyMarkup: new ReplyKeyboardRemove());
    }

    public async Task<Message> ForceUpdateCommand(Message message)
    {
        return await botClient.SendMessage(chatId: message.Chat.Id,
                                text: "Запуск ...");
    }

    public async Task<Message> LastUpdateCommand(Message message)
    {
        string replyText = $"Функционал отключен";
        return await botClient.SendMessage(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);

    }

    public async Task<Message> TrackedArtistsForProviderCommand(Update update, string providerName)
    {
        string replyText = $"Функционал отключен";
        return await botClient.SendMessage(chatId: update.Message.Chat.Id,
                                        text: replyText, ParseMode.Html);
    }

    public async Task<Message> ProviderListCommand(Message message)
    {
        string replyText = $"Функционал отключен";
        return await botClient.SendMessage(chatId: message.Chat.Id,
                                        text: replyText, ParseMode.Html);
    }
}
