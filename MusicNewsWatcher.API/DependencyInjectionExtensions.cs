using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.Controllers;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.MusicProviders;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using Telegram.Bot;

namespace MusicNewsWatcher.API;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMusicNewsWatcherApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizeMiddlewareOptions>(configuration.GetSection(nameof(AuthorizeMiddlewareOptions)));
        services.Configure<TelegramBotConfiguration>(configuration.GetSection(nameof(TelegramBotConfiguration)));

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


        services.AddSingleton<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
        services.AddSingleton<ITelegramBotClient>(options =>
        {
            return new TelegramBotClient(configuration.GetSection("TelegramBotConfiguration:ApiKey").Value ?? throw new ArgumentNullException());
        });
        services.AddSingleton<TelegramBotRoutes>();
        services.AddSingleton<TelegramBotCommandHandlers>();
        services.AddSingleton<MusicNewsWatcherTelegramBot>();

        return services;
    }
}