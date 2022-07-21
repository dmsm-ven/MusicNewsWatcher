using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;

public static class Program
{
    static MusicNewsWatcherTelegramBot bot;
    static MusicUpdateManager updateManager;
    static IHost host;

    public static async Task Main(string[] args)
    {
        ConfigureServices();

        await StartBot();
        await StartUpdateManager();

        Console.Write("press any key to stop");
    }

    private static void ConfigureServices()
    {
        host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>(options =>
                {
                    string connectionString = context.Configuration["ConnectionStrings:default"];
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });

                services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
                services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
                services.AddSingleton<MusicUpdateManager>();
                services.AddSingleton<MusicNewsWatcherTelegramBot>();
            })
            .Build();
    }

    private static async Task StartBot()
    {
        string apiToken = string.Empty;
        long consumerId = 0;
        bot = host.Services.GetRequiredService<MusicNewsWatcherTelegramBot>();
        await bot.Start();
        Console.WriteLine($"Telegram bot listening, account @{bot.AccountName}");
    }

    private static async Task StartUpdateManager()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        TimeSpan updateInterval = TimeSpan.FromMinutes(30);
        //получаем интервал обновления
        updateManager = host.Services.GetRequiredService<MusicUpdateManager>();
        updateManager.OnNewAlbumsFound += UpdateManager_OnNewAlbumsFound;
        updateManager.Start(updateInterval);
        Console.WriteLine($"Auto-update enabled, interval: {(int)updateInterval.TotalMinutes} min.");
    }

    private static async void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        await bot.NotifyAboutNewAlbums(e.Provider, e.Artist, e.NewAlbums);
    }
}


