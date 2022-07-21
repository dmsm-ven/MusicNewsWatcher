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

    private User? BotInfo { get; set; }

    public MusicNewsWatcherTelegramBot(string apiToken, long consumerId, bool createStarted = false)
    {
        bot = new TelegramBotClient(apiToken);
        cts = new CancellationTokenSource();
        this.consumerId = consumerId;
        if (createStarted)
        {
            Start();
        }
    }

    public void Start()
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
       
    }

    public async Task SendAsBot(string message)
    {
        var answer = await bot.SendTextMessageAsync(consumerId, message, ParseMode.Html);
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
}


