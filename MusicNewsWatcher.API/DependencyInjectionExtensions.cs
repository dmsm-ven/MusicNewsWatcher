using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.MusicProviders;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.API.Services;

namespace MusicNewsWatcher.API;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMusicNewsWatcherApi(this IServiceCollection services, IConfiguration configuration)
    {

        //MusicNewsWatcher.BotService code moved here
        services.AddDbContextFactory<MusicWatcherDbContext>(options =>
        {
            string? connectionString = configuration["ConnectionStrings:default"];
            options.UseNpgsql(connectionString);
        });

        services.AddHttpClient();
        services.AddScoped<MusicNewsCrawler>();
        services.AddScoped<MusicUpdateManager>();
        services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
        services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();


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