using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.TelegramBot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
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

                services.AddHostedService<WindowsBackgroundService>();
            })
            .Build();

        //запускает телеграм бота для оповещения об обновлениях/ответа на запросы
        await host.Services.GetRequiredService<MusicNewsWatcherTelegramBot>().Start();

        //Должен быть последним, что бы обрабатывать завершение от systemctl stop
        await host.RunAsync();
    }
}


public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly MusicUpdateManager updateManager;

    public WindowsBackgroundService(MusicUpdateManager updateManager)
    {
        this.updateManager = updateManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await updateManager.Start();

            while (!stoppingToken.IsCancellationRequested)
            {               
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Environment.Exit(1);
        }
    }
}

