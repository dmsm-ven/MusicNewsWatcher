using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.Extensions;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class TelegramBotCommandHandlers(ITelegramBotClient botClient, MusicWatcherDbContext dbContext)
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

        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: usageMessage,
                                                    replyMarkup: new ReplyKeyboardRemove());
    }

    public async Task<Message> ForceUpdateCommand(Message message)
    {
        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                text: "Запуск ...", (int?)ParseMode.Html);
    }

    public async Task<Message> LastUpdateCommand(Message message)
    {
        string? lastUpdateString = dbContext.Settings.Find("LastFullUpdateDateTime")?.Value;

        bool emptyLastUpdate = false;
        DateTimeOffset lastUpdate, nextUpdate;
        if (!DateTimeOffset.TryParse(lastUpdateString, out lastUpdate))
        {
            lastUpdate = new DateTimeOffset();
            emptyLastUpdate = true;
        }
        TimeSpan updateInterval = TimeSpan.FromMinutes(int.Parse(dbContext.Settings.Find("UpdateManagerIntervalInMinutes")?.Value ?? "0"));
        int updateIntervalMinutes = (int)updateInterval.TotalMinutes;
        nextUpdate = lastUpdate.AddMinutes(updateIntervalMinutes);
        int nextUpdateLeft = (int)(nextUpdate - DateTimeOffset.UtcNow).TotalMinutes;

        if (!emptyLastUpdate)
        {
            var sb = new StringBuilder()
                .AppendLine($"Интервал обновления: <b>{updateIntervalMinutes} мин.</b>")
                .AppendLine($"Дата/время последнего обновления: <b>{lastUpdate.ToLocalRuDateAndTime()}</b> ({lastUpdateString})")
                .Append($"Следующий запуск: <b>{nextUpdate.ToLocalRuDateAndTime()}</b> (через {nextUpdateLeft} мин.)");


            string replyText = sb.ToString();
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                            text: replyText, (int?)ParseMode.Html);
        }
        else
        {
            string replyText = $"Ошибка считывания даты последнего обновления: '{lastUpdateString}'";
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                            text: replyText, (int?)ParseMode.Html);
        }

    }

    public async Task<Message> TrackedArtistsForProviderCommand(Update update, string providerName)
    {
        if (update == null || update.CallbackQuery == null)
        {
            throw new ArgumentNullException(nameof(update));
        }

        var chatId = update.CallbackQuery.From.Id;

        var dbProvider = dbContext.MusicProviders.Include(p => p.Artists)
            .FirstOrDefault(pr => pr.Name == providerName);

        if (dbProvider != null)
        {
            string[] artistLines = dbProvider.Artists
                .OrderBy(a => a.Name)
                .Select((artist, i) => $"[{i + 1}] {artist.Name}")
                .ToArray();

            string replyText = string.Join("\r\n", artistLines);

            return await botClient.SendTextMessageAsync(chatId: chatId,
                                            text: replyText,
                                            replyMarkup: new ReplyKeyboardRemove());
        }

        return await botClient.SendTextMessageAsync(chatId: chatId, text: $"Провайдер '{providerName}' не найден", replyMarkup: new ReplyKeyboardRemove());
    }

    public async Task<Message> ProviderListCommand(Message message)
    {
        var providersData = dbContext.MusicProviders.Select(mp => new
        {
            mp.Name,
            TrackedArtists = mp.Artists.Count()
        }).ToArray();

        var buttons = providersData.Select(provider => new InlineKeyboardButton($"{provider.Name} ({provider.TrackedArtists} исп.)")
        {
            CallbackData = $"/tracked_artists_for_provider {provider.Name}"
        }).ToArray();

        InlineKeyboardMarkup inlineKeyboard = new(buttons);

        return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: "У какого провайдера ?",
                                                    replyMarkup: inlineKeyboard);
    }
}
