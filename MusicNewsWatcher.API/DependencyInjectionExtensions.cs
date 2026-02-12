using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.Models;
using MusicNewsWatcher.API.MusicProviders;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using System.Diagnostics;
using Telegram.Bot;

namespace MusicNewsWatcher.API;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
        services.AddSingleton<IReadOnlyDictionary<TelegramBotCommand, Func<Task<string>>>>((x) =>
        {
            var scope = x.CreateScope();
            IMusicNewsMessageFormatter formatter = scope.ServiceProvider.GetRequiredService<IMusicNewsMessageFormatter>();
            var updateManager = scope.ServiceProvider.GetRequiredService<MusicUpdateManager>();
            var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>();

            var handlers = new Dictionary<TelegramBotCommand, Func<Task<string>>>()
            {
                [TelegramBotCommand.ExecutForceUpdate] = async () =>
                {
                    if (!updateManager.CrawlerInProgress)
                    {
                        var sw = Stopwatch.StartNew();
                        await updateManager.RunCrawler();
                        return $"Переобход выполнен за {(int)sw.Elapsed.TotalSeconds} секунд";

                    }
                    return "Переообход уже в процессе выполнения";
                },
                [TelegramBotCommand.ShowLastUpdate] = () =>
                {
                    return Task.FromResult($"Последний перееобход был {updateManager.LastUpdate.ToLocalTime()}");
                },
                [TelegramBotCommand.ShowTrackedArtists] = async () =>
                {
                    using var db = dbFactory.CreateDbContext();

                    //получаем список вида Провайдер -> Список отслеживаемых исполнителей
                    var dbData = await db.MusicProviders
                        .AsNoTracking()
                        .Include(x => x.Artists)
                        .Select(i => new
                        {
                            ProviderName = i.Name,
                            ArtistNames = i.Artists.Select(a => a.Name).ToArray()
                        })
                        .ToArrayAsync();

                    string message = formatter.BuildTrackedArtistsListMessage(dbData.ToDictionary(i => i.ProviderName, i => i.ArtistNames));
                    return message;
                }
            };
            return handlers;
        });

        services.AddSingleton<MusicWatcherTelegramBotClient>(x =>
        {
            var scope = x.CreateScope();

            var options = configuration
                .GetSection(nameof(MusicWatcherTelegramBotConfiguration))
                .Get<MusicWatcherTelegramBotConfiguration>() ?? throw new ArgumentNullException("Ошибка конфигурации TG бота");

            var routeHandlers = scope.ServiceProvider.GetRequiredService<IReadOnlyDictionary<TelegramBotCommand, Func<Task<string>>>>();
            var client = new TelegramBotClient(options!.ApiKey);
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MusicWatcherTelegramBotClient>>();

            return new MusicWatcherTelegramBotClient(client, logger, options, routeHandlers);
        });

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

        services.AddSingleton<CrawlerHttpClientProviderFactory>();

        services.AddSingleton<MusicNewsCrawler>();

        services.AddSingleton<MusicUpdateManager>();
        services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
        services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();


        return services;
    }
}
