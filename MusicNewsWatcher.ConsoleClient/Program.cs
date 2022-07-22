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
        host = CreateHostBuilder(args).Build();
       
        await StartBot();

        StartUpdateManager();

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder()
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
                services.AddTransient<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
                services.AddSingleton<MusicNewsWatcherTelegramBot>();
            });
    
    }

    private static async Task StartBot()
    {
        bot = host.Services.GetRequiredService<MusicNewsWatcherTelegramBot>();
        await bot.Start();
        Console.WriteLine($"Telegram bot listening, account @{bot.AccountName}");
    }

    private static void StartUpdateManager()
    {
        //получаем интервал обновления
        updateManager = host.Services.GetRequiredService<MusicUpdateManager>();
        updateManager.OnNewAlbumsFound += UpdateManager_OnNewAlbumsFound;
        updateManager.Start();
        Console.WriteLine($"Auto-update enabled, interval: {(int)updateManager.UpdateInterval.TotalMinutes} min.");
    }

    private static async void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        await bot.NotifyAboutNewAlbums(e.Provider, e.Artist, e.NewAlbums);
    }
}


