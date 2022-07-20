using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static MusicNewWatcher.TelegramBot.UpdateHandlers;

namespace MusicNewWatcher.TelegramBot;

public class MusicNewsWatcherTelegramBot : IDisposable
{
    private readonly TelegramBotClient bot;
    private readonly CancellationTokenSource cts;

    public MusicNewsWatcherTelegramBot(string apiToken)
    {
        bot = new TelegramBotClient(apiToken);
        cts = new CancellationTokenSource();
    }

    public async Task StartAsync()
    {
        var me = await bot.GetMeAsync();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        bot.StartReceiving(updateHandler: UpdateHandlers.HandleUpdateAsync,
                           pollingErrorHandler: UpdateHandlers.PollingErrorHandler,
                           cancellationToken: cts.Token);

        Console.WriteLine($"Bot Entered as @{me.Username}");
    }

    public async Task SendTextMessageAsync(string message)
    {
        await bot.SendTextMessageAsync(null, message);
    }

    public void Stop()
    {
        cts.Cancel();
    }

    public void Dispose()
    {
        cts?.Dispose();
    }
}


