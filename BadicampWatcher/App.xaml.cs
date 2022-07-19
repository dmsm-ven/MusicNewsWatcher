using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using MusicNewsWatcher.Services;
using MusicNewsWatcher.ViewModels;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace MusicNewsWatcher;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static string CacheDirectory
    {
        get
        {
            string cacheDir = Path.Combine(Environment.CurrentDirectory, "cache");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }
            return cacheDir;
        }
    }
    public static string DownloadDirectory
    {
        get
        {
            string downloadDir = Path.Combine(Environment.CurrentDirectory, "downloads");
            if (!Directory.Exists(downloadDir))
            {
                Directory.CreateDirectory(downloadDir);
            }
            return downloadDir;
        }
    }
    
    public static IHost HostContainer { get; private set; }

    readonly Mutex _mutex;
    TaskbarIcon tbi;
    ISettingsStorage settingsStorage;

    public App()
    {
        _mutex = new Mutex(false, "MusicNewsWatcherWpfApp");

        if (!_mutex.WaitOne(500, false))
        {
            Application.Current.Shutdown();
        }

        HostContainer = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>();
                services.AddToasts();

                services.AddSingleton<ISettingsStorage>(x => new JsonSettingsStorage("app_settings.json"));                
                services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
                services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
                services.AddSingleton<MusicDownloadManager>(x => new MusicDownloadManager(App.DownloadDirectory));
                services.AddSingleton<MusicUpdateManager>();
                services.AddSingleton<MainWindowViewModel>();
            })
            .Build();

        
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await HostContainer.StartAsync();

        settingsStorage = HostContainer.Services.GetRequiredService<ISettingsStorage>();
        SettingsRoot settingsRoot = settingsStorage.Load();

        var mainWindow = new MainWindow();
        mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.Top = settingsRoot.MainWindowRectangle.Y;
        mainWindow.Left = settingsRoot.MainWindowRectangle.X;
        mainWindow.Width = settingsRoot.MainWindowRectangle.Width;
        mainWindow.Height = settingsRoot.MainWindowRectangle.Height;
        mainWindow.DataContext = HostContainer.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Show();

        //ConfigureNotifyIcon();
    }

    private void ConfigureNotifyIcon()
    {
        if (tbi == null)
        {
            tbi = new TaskbarIcon();

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
        }
    }

    protected override void OnDeactivated(EventArgs e)
    {
        var rect = new Rectangle()
        {
            Location = new System.Drawing.Point((int)MainWindow.Left, (int)MainWindow.Top),
            Size = new System.Drawing.Size((int)MainWindow.Width, (int)MainWindow.Height)
        };

        settingsStorage.SetValue(nameof(SettingsRoot.MainWindowRectangle), rect);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await HostContainer.StopAsync();
        base.OnExit(e);
    }
}

public static class DIExtensions
{
    public static void AddToasts(this IServiceCollection services)
    {
        var notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 25,
                offsetY: 25);

            cfg.DisplayOptions.Width = 200;

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(60),
                maximumNotificationCount: MaximumNotificationCount.FromCount(4));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        services.AddSingleton<Notifier>(notifier);
    }
}



