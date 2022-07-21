using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static MusicNewsWatcher.TelegramBot.UpdateHandlers;

namespace MusicNewsWatcher.TelegramBot;

public class MusicNewsWatcherTelegramBot : IDisposable
{
    public bool IsStarted { get; private set; }

    private TelegramBotClient bot;
    private CancellationTokenSource cts;
    private string? apiToken;
    private string? consumerId;
    private readonly IMusicNewsMessageFormatter formatter;
    internal static IDbContextFactory<MusicWatcherDbContext> DbFactory { get; private set; }
    User? botInfo;

    public string AccountName => botInfo?.Username ?? "Not connected";

    public MusicNewsWatcherTelegramBot(IMusicNewsMessageFormatter formatter,  IDbContextFactory<MusicWatcherDbContext> dbFactory)
    {
        //TODO убрать статический метод
        this.formatter = formatter;
        DbFactory = dbFactory;
    }

    public async Task Start()
    {
        if (IsStarted) { return; }

        using (var db = await DbFactory.CreateDbContextAsync())
        {
            this.apiToken = db.Settings.Find("TelegramApiToken")?.Value;
            this.consumerId = db.Settings.Find("TelegramChatId")?.Value;
        }

        if (string.IsNullOrWhiteSpace(apiToken) || string.IsNullOrWhiteSpace(consumerId))
        {
            throw new ArgumentNullException(nameof(apiToken), "Telegram api token not provided");
        }

        bot = new TelegramBotClient(apiToken);
        cts = new CancellationTokenSource();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        bot.StartReceiving(updateHandler: UpdateHandlers.HandleUpdateAsync,
                           pollingErrorHandler: UpdateHandlers.PollingErrorHandler,
                           cancellationToken: cts.Token);

        botInfo = await bot.GetMeAsync(cts.Token);

        IsStarted = true;
    }

    public void Stop()
    {
        cts.Cancel();
    }

    public void Dispose()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    public async Task NotifyAboutNewAlbums(string provider, ArtistDto artist, IEnumerable<AlbumDto> albums)
    {
        var text = formatter.BuildNewAlbumsFoundMessage(provider, artist, albums);

        await bot.SendTextMessageAsync(consumerId, text, ParseMode.Html);
    }
}



