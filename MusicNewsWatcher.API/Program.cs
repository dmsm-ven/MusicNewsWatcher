using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMusicNewsWatcherApi(this IServiceCollection services, IConfiguration configuration)
    {
        //MusicNewsWatcher.BotService code moved here
        services.AddDbContextFactory<MusicWatcherDbContext>(options =>
        {
            string? connectionString = context.Configuration["ConnectionStrings:default"];
            options.UseNpgsql(connectionString);
        });

        /*
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
        services.AddSingleton<MusicNewsWatcherTelegramBot>(); */
        return services;
    }
}