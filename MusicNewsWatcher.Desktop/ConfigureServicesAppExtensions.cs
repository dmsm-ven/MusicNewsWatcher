using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Configuration;
using MusicNewsWatcher.TelegramBot;
using MusicNewsWatcher.TelegramBot.MessageFormatters;
using MusicNewWatcher.BL.MusicProviders;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace MusicNewsWatcher.Desktop;

public static class ConfigureServicesAppExtensions
{
    public static void AddMusicProviders(this IServiceCollection services)
    {
        services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
        services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
    }

    public static void AddNotifyIcon(this IServiceCollection services)
    {
        var tbi = new TaskbarIcon();

        var tbiMenu = new ContextMenu();
        var showMenuItem = new MenuItem() { Header = "Отобразить" };
        showMenuItem.Click += (o, e) => Application.Current.MainWindow.Show();
        var exitMenuItem = new MenuItem() { Header = "Выход" };
        exitMenuItem.Click += (o, e) =>
        {
            Application.Current.Shutdown();
        };

        tbiMenu.Items.Add(showMenuItem);
        tbiMenu.Items.Add(new Separator()); // null = separator
        tbiMenu.Items.Add(exitMenuItem);

        tbi.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/logo.ico")).Stream);
        tbi.ToolTipText = "Парсер музыки";
        tbi.ContextMenu = tbiMenu;

        services.AddSingleton<TaskbarIcon>(tbi);
    }

    public static void AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
        services.Configure<TelegramBotConfiguration>(configuration.GetSection("TelegramBot"));
        services.AddTransient<MusicNewsWatcherTelegramBot>();
        services.AddSingleton<Func<MusicNewsWatcherTelegramBot>>(x => () => x.GetRequiredService<MusicNewsWatcherTelegramBot>());
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
            cfg.DisplayOptions.TopMost = false;

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(60),
                maximumNotificationCount: MaximumNotificationCount.FromCount(4));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        services.AddSingleton<IToastsNotifier>(x => new DewCrewToastsNotifier(notifier));
    }
}



