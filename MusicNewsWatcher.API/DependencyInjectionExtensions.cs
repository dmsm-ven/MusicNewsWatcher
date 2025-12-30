using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.Controllers;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.Services;

namespace MusicNewsWatcher.API;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMusicNewsWatcherApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthorizeMiddlewareOptions>(configuration.GetSection(nameof(AuthorizeMiddlewareOptions)));

        //MusicNewsWatcher.BotService code moved here
        services.AddDbContextFactory<MusicWatcherDbContext>(options =>
        {
            string? connectionString = configuration["ConnectionStrings:default"];
            options.UseNpgsql(connectionString);
        });
        services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
        services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
        services.AddSingleton<MusicNewsCrawler>();
        services.AddSingleton<MusicUpdateManager>();

        //services.AddTransient<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
        //services.Configure<TelegramBotConfiguration>(context.Configuration.GetSection("TelegramBot"));
        //services.AddSingleton<ITelegramBotClient>(options =>
        //{
        //    var tgConfig = new TelegramBotConfiguration();
        //    context.Configuration.GetSection("TelegramBot").Bind(tgConfig);
        //    return new TelegramBotClient(tgConfig.ApiKey);
        //});
        //services.AddSingleton<TelegramBotRoutes>();
        //services.AddSingleton<TelegramBotCommandHandlers>();
        //services.AddSingleton<MusicNewsWatcherTelegramBot>(); 
        return services;
    }
}