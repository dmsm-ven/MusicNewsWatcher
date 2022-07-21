using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Services;
using MusicNewsWatcher.TelegramBot;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace MusicNewsWatcher;

public static class ConfigureServicesAppExtensions
{   
    public static void AddTelegramBot(this IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
        services.AddTransient<MusicNewsWatcherTelegramBot>(x =>
        {            
            using var db = x.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>().CreateDbContext();
            string apiToken = (db.Settings?.Find("TelegramApiToken")?.Value) ?? context.Configuration["Telegram:API_TOKEN"];
            long consumerId = long.Parse(context.Configuration["Telegram:ConsumerChatId"]);

            var bot = new MusicNewsWatcherTelegramBot(apiToken, consumerId, createStarted: true);
            return bot;
        });
        services.AddSingleton<Func<MusicNewsWatcherTelegramBot>>(x => () => x.GetRequiredService<MusicNewsWatcherTelegramBot>());
    }

    public static void AddNotifyIcon(this IServiceCollection services)
    {
        var tbi = new TaskbarIcon();

        var tbiMenu = new ContextMenu();
        var showMenuItem = new MenuItem() { Header = "Отобразить" };
        showMenuItem.Click += (o, e) => Application.Current.MainWindow.Show();
        var exitMenuItem = new MenuItem() { Header = "Выход" };
        exitMenuItem.Click += (o, e) => Application.Current.Shutdown();

        tbiMenu.Items.Add(showMenuItem);
        tbiMenu.Items.Add(new Separator()); // null = separator
        tbiMenu.Items.Add(exitMenuItem);

        tbi.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/logo.ico")).Stream);
        tbi.ToolTipText = "Парсер музыки";
        tbi.ContextMenu = tbiMenu;

        services.AddSingleton<TaskbarIcon>(tbi);
    }

    public static void AddToasts(this IServiceCollection services)
    {
        var notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 25,
                offsetY: 25);

            cfg.DisplayOptions.Width = 500;

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(60),
                maximumNotificationCount: MaximumNotificationCount.FromCount(4));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        services.AddSingleton<Notifier>(notifier);
    }
}



