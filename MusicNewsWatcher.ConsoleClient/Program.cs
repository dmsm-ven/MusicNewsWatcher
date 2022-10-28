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
       
        //запускает телеграм бота для оповещения об обновлениях/ответа на запросы
        await StartBot();

        //Запускаем службу обновления (парсера) музыки
        StartUpdateManager();

        //Должен быть последним, что бы обрабатывать завершение от systemctl stop
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
                    Console.WriteLine(connectionString);

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
        bot.OnParsingReceived += Bot_OnParsingReceived;
        await bot.Start();
        Console.WriteLine($"Telegram bot listening, account @{bot.AccountName}");
    }

    private static async void Bot_OnParsingReceived()
    {
        if (!updateManager.CrawlerInProgress)
        {
            await updateManager.CheckUpdatesAllAsync();

            await bot.SendCustomMessage("Переобход выполнен");
        }
        else
        {
            await bot.SendCustomMessage("Переобход уже в процессе выполнения");
        }
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


