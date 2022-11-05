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
        host = Host.CreateDefaultBuilder(args)
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
            })
            .Build();

        //запускает телеграм бота для оповещения об обновлениях/ответа на запросы
        await host.Services.GetRequiredService<MusicNewsWatcherTelegramBot>().Start();

        //Должен быть последним, что бы обрабатывать завершение от systemctl stop
        await host.RunAsync();
    }
}


