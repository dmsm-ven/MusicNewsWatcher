using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core;
using System.Globalization;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MusicNewsWatcher.TelegramBot;

public class TelegramBotCommandHandlers
{
    private readonly ITelegramBotClient botClient;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContextFactory;

    public TelegramBotCommandHandlers(ITelegramBotClient botClient,
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory)
    {
        this.botClient = botClient;
        this.dbContextFactory = dbContextFactory;
    }

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
                                text: "Запуск ...", ParseMode.Html);
    }

    public async Task<Message> LastUpdateCommand(Message message)
    {
        using var db = await dbContextFactory.CreateDbContextAsync();

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

    public async Task<Message> TrackedArtistsForProviderCommand(Update update, string providerName)
    {
        if (update == null || update.CallbackQuery == null)
        {
            throw new ArgumentNullException(nameof(update));
        }

        var chatId = update.CallbackQuery.From.Id;

        using var db = dbContextFactory.CreateDbContext();

        var dbProvider = db.MusicProviders.Include(p => p.Artists)
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
        using var db = await dbContextFactory.CreateDbContextAsync();

        var providersData = db.MusicProviders.Select(mp => new
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
