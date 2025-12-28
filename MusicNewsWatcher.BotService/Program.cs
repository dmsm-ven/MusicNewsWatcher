using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.BotService.HostedServices;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using MusicNewsWatcher.BL;
using MusicNewsWatcher.BL.MusicProviders;
using Telegram.Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>(options =>
                {
                    string? connectionString = context.Configuration["ConnectionStrings:default"];
                    options.UseNpgsql(connectionString);
                });

                services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
                services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
                services.AddSingleton<IMusicNewsCrawler, EfMusicNewsCrawler>();
                services.AddSingleton<MusicUpdateManager>();

                services.AddTransient<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();

                services.Configure<TelegramBotConfiguration>(context.Configuration.GetSection("TelegramBot"));
                services.AddSingleton<ITelegramBotClient>(options =>
                {
                    var tgConfig = new TelegramBotConfiguration();
                    context.Configuration.GetSection("TelegramBot").Bind(tgConfig);
                    return new TelegramBotClient(tgConfig.ApiKey);
                });
                services.AddSingleton<TelegramBotRoutes>();
                services.AddSingleton<TelegramBotCommandHandlers>();
                services.AddSingleton<MusicNewsWatcherTelegramBot>();

                services.AddHostedService<TelegramBotHostedService>();
                services.AddHostedService<CrawlerHostedService>();
            })
            .Build();

        //Должен быть последним, что бы обрабатывать завершение от systemctl stop
        await host.RunAsync();
    }
}

