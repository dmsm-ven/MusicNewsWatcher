using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicNewsWatcher.Core;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MusicNewsWatcher.TelegramBot;

public class MusicNewsWatcherTelegramBot : IDisposable
{
    public bool IsStarted { get; private set; }

    private readonly MusicUpdateManager updateManager;
    private readonly IMusicNewsMessageFormatter formatter;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;

    private readonly CancellationTokenSource cts;
    private readonly string? apiToken;
    private readonly string? consumerId;

    private TelegramBotClient botClient;
    
    public MusicNewsWatcherTelegramBot(
        MusicUpdateManager updateManager,
        IMusicNewsMessageFormatter formatter,  
        IDbContextFactory<MusicWatcherDbContext> dbFactory,
        IConfiguration configuration)
    {
        this.updateManager = updateManager;
        this.formatter = formatter;
        this.dbFactory = dbFactory;

        cts = new CancellationTokenSource();
        apiToken = configuration["TelegramBot:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentNullException(nameof(apiToken), "Telegram api token not provided");
        }

        consumerId = configuration["TelegramBot:ClientId"];
        if (string.IsNullOrWhiteSpace(consumerId))
        {
            throw new ArgumentNullException(nameof(consumerId), "Telegram consumerId not provided");
        }
    }

    public async Task Start()
    {
        if (IsStarted)
        {
            throw new NotSupportedException();
        }

        botClient = new TelegramBotClient(apiToken);
    
        var thBotMessageHandler = new TelegramBotRoutes(botClient, dbFactory);
        botClient.StartReceiving(thBotMessageHandler);


        //botInfo = await botClient.GetMeAsync(cts.Token); тут например можем узнать название бота


        updateManager.OnNewAlbumsFound += (o, e) =>
        {
            var text = formatter.BuildNewAlbumsFoundMessage(e.Provider, e.Artist, e.NewAlbums);
            botClient.SendTextMessageAsync(consumerId, text, ParseMode.Html);
        };
        thBotMessageHandler.OnForceUpdateCommandRecevied += async () =>
        {
            await updateManager.CheckUpdatesAllAsync();
        };

        updateManager.Start();

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
        updateManager.Dispose();
    }
}