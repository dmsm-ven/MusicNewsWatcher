using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.BackgroundServices;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.Models;
using MusicNewsWatcher.API.MusicProviders;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.Core;
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
                [TelegramBotCommand.ExecuteForceUpdate] = async () =>
                {
                    if (!updateManager.CrawlerInProgress)
                    {
                        var sw = Stopwatch.StartNew();
                        await updateManager.RunCrawler();
                        return $"Переобход выполнен за {(int)sw.Elapsed.TotalSeconds} секунд";
                    }
                    return "Переообход сейчас в процессе выполнения";
                },
                [TelegramBotCommand.ShowLastUpdate] = () =>
                {
                    if (updateManager.CrawlerInProgress)
                    {
                        return Task.FromResult("Переообход сейчас в процессе выполнения");
                    }

                    var localScope = x.CreateScope();
                    var updateInterval = localScope.ServiceProvider.GetRequiredService<IOptionsMonitor<CrawlerConfiguration>>().CurrentValue.CheckInterval;
                    string lastExecDt = updateManager.LastUpdate.ToRussianLocalTime();
                    int elapsedMinutes = (int)(DateTime.UtcNow - updateManager.LastUpdate).TotalMinutes;
                    int nextExecInMinutes = Math.Max((int)updateInterval.TotalMinutes - elapsedMinutes, 0);

                    string msg = $"Последний перееобход был {lastExecDt} ({elapsedMinutes} мин. назад). Следующий запуск через: {nextExecInMinutes} мин.";
                    return Task.FromResult(msg);
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
                },
                [TelegramBotCommand.ShowLastParsedAlbums] = async () =>
                {
                    using var db = dbFactory.CreateDbContext();
                    var artistNames = await db.Artists
                        .AsNoTracking()
                        .ToDictionaryAsync(i => i.ArtistId, i => i.Name);

                    var dbData = (await db.Albums
                    .AsNoTracking()
                    .OrderByDescending(album => album.AlbumId)
                    .Take(10)
                    .ToArrayAsync())
                    .Select(i => new LastParsedAlbumInfo()
                    {
                        AlbumName = i.Title,
                        ArtistName = artistNames[i.ArtistId],
                        CreatedAt = i.Created,
                        Uri = i.Uri
                    })
                    .ToArray();

                    string message = formatter.BuilderLastAlbumsMessage(dbData);
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
