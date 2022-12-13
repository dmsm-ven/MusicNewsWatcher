using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;
using Telegram.Bot;

public sealed class ServiceBackgroundWorker : BackgroundService
{
    private readonly MusicNewsWatcherTelegramBot botClient;
    private readonly MusicUpdateManager updateManager;
    private readonly IConfiguration configuration;
    private readonly IMusicNewsMessageFormatter telegramMessageFormatter;

    public ServiceBackgroundWorker(MusicNewsWatcherTelegramBot botClient,
        MusicUpdateManager updateManager,
        IConfiguration configuration,
        IMusicNewsMessageFormatter telegramMessageFormatter)
    {
        this.botClient = botClient;
        this.updateManager = updateManager;
        this.configuration = configuration;
        this.telegramMessageFormatter = telegramMessageFormatter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            updateManager.OnNewAlbumsFound += (o, e) =>
            {
                var text = telegramMessageFormatter.BuildNewAlbumsFoundMessage(e.Provider, e.Artist, e.NewAlbums);
                botClient.SendTextMessageAsync(text);
            };
            updateManager.Start();

            var apiToken = configuration["TelegramBot:ApiKey"];
            var consumerId = configuration["TelegramBot:ClientId"];

            botClient.Start(apiToken, consumerId);
            botClient.OnForceUpdateCommandReceved += async () => await updateManager.CheckUpdatesAllAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Environment.Exit(1);
        }
    }
}

