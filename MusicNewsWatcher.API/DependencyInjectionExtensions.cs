using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.Models;
using MusicNewsWatcher.API.MusicProviders;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using Telegram.Bot;

namespace MusicNewsWatcher.API;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MusicWatcherTelegramBotConfiguration>(configuration.GetSection(nameof(MusicWatcherTelegramBotConfiguration)));
        services.AddSingleton<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
        services.AddSingleton<ITelegramBotClient>(provider =>
        {
            var options = configuration
                .GetSection(nameof(MusicWatcherTelegramBotConfiguration))
                .Get<MusicWatcherTelegramBotConfiguration>();
            return new TelegramBotClient(options.ApiKey);
        });
        services.AddSingleton<MusicWatcherTelegramBotClient>();

        return services;
    }
    public static IServiceCollection AddMusicNewsWatcherApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizeMiddlewareOptions>(configuration.GetSection(nameof(AuthorizeMiddlewareOptions)));


        //MusicNewsWatcher.BotService code moved here
        services.AddDbContextFactory<MusicWatcherDbContext>(options =>
        {
            string? connectionString = configuration["ConnectionStrings:default"];
            options.UseNpgsql(connectionString);
        });

        services.AddHttpClient();
        services.AddSingleton<MusicNewsCrawler>();
        services.AddSingleton<MusicUpdateManager>();
        services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
        services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();


        return services;
    }
}