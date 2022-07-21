using MusicNewsWatcher.Core;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static MusicNewsWatcher.TelegramBot.UpdateHandlers;

namespace MusicNewsWatcher.TelegramBot;

public class MusicNewsWatcherTelegramBot : IDisposable
{
    private readonly TelegramBotClient bot;
    private readonly CancellationTokenSource cts;
    private readonly long consumerId;
    private readonly IMusicNewsMessageFormatter formatter;
    User? botInfo;

    public string AccountName => botInfo?.Username ?? "Not connected";

    public MusicNewsWatcherTelegramBot(string apiToken, long consumerId, IMusicNewsMessageFormatter formatter)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentNullException(nameof(apiToken), "Telegram api token not provided");
        }

        if (consumerId == 0)
        {
            throw new ArgumentException(nameof(consumerId), "bot consumer not provided");
        }

        bot = new TelegramBotClient(apiToken);
        cts = new CancellationTokenSource();
        this.formatter = formatter;
    }

    public async Task Start()
    {
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
        var text = formatter.NewAlbumsFoundMessage(provider, artist, albums);

        await bot.SendTextMessageAsync(consumerId, text, ParseMode.Html);
    }
}



