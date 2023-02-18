global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using MusicNewsWatcher.Core;
global using MusicNewsWatcher.Desktop.Models;
global using MusicNewsWatcher.Desktop.Services;
global using MusicNewsWatcher.Desktop.ViewModels;
global using MusicNewsWatcher.Desktop.Views;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using ToastNotifications;
global using ToastNotifications.Messages;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Services;
using MusicNewsWatcher.TelegramBot;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace MusicNewsWatcher.Desktop;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost HostContainer { get; private set; }
    public static Mutex mutex;

    public App()
    {
        mutex = new Mutex(false, "MusicNewsWatcherWpfApp");

        if (!mutex.WaitOne())
        {
            Application.Current.Shutdown();
        }

        HostContainer = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(options =>
            {
                //options.AddUserSecrets(this.GetType().Assembly);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>(options =>
                {
                    string connectionString = context.Configuration.GetConnectionString("default");
                    options.UseNpgsql(connectionString);
                });

                services.AddToasts();
                //services.AddNotifyIcon();
                services.AddTelegramBot(context);

                services.AddTransient<ISyncLibraryTracker, SyncLibraryTracker>();
                services.AddTransient<IDialogWindowService, DialogWindowService>();

                services.AddTransient<AddNewArtistDialogViewModel>();
                services.AddTransient<AddNewArtistDialog>();

                services.AddMusicProviders();
                services.AddSingleton<MusicDownloadManager>();
                services.AddSingleton<MusicUpdateManager>();

                services.AddSingleton<SettingsWindowViewModel>();
                services.AddTransient<SettingsWindow>();

                services.AddTransient<SyncLibraryWindow>();
                services.AddTransient<SyncLibraryWindowViewModel>();

                services.AddSingleton<MainWindowViewModel>();
            })
            .Build();

    }

    protected override async void OnStartup(StartupEventArgs e)
    {

        //Окно занимает 85% экрана
        double sizeRatio = 0.85;

        var mainWindow = new MainWindow();
        mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.DataContext = HostContainer.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Width = SystemParameters.PrimaryScreenWidth * sizeRatio;
        mainWindow.Height = SystemParameters.PrimaryScreenHeight * sizeRatio;
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        mutex?.ReleaseMutex();
        await HostContainer.StopAsync();
        HostContainer.Dispose();
    }
}

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

    public static void AddTelegramBot(this IServiceCollection services, HostBuilderContext context)
    {
        services.AddTransient<IMusicNewsMessageFormatter, MusicNewsHtmlMessageFormatter>();
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



