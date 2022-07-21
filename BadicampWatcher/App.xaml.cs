global using Microsoft.EntityFrameworkCore;
global using MusicNewsWatcher.Core;
global using System.Collections.Generic;
global using System;
global using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Services;
using MusicNewsWatcher.ViewModels;
using MusicNewsWatcher.Views;
using System.Threading;
using System.Windows;
using ToastNotifications;


namespace MusicNewsWatcher;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost HostContainer { get; private set; }

    readonly Mutex _mutex;

    public App()
    {
        _mutex = new Mutex(false, "MusicNewsWatcherWpfApp");

        if (!_mutex.WaitOne(500, false))
        {
            Application.Current.Shutdown();
        }

        HostContainer = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(options =>
            {
                options.AddUserSecrets(this.GetType().Assembly);
            })
            .ConfigureServices((context,services) =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>();
                services.AddToasts();
                services.AddNotifyIcon();
                services.AddTelegramBot(context);
                
                services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
                services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
                services.AddSingleton<MusicDownloadManager>(x => new MusicDownloadManager(FileBrowserHelper.DownloadDirectory));
                services.AddSingleton<MusicUpdateManager>();
                services.AddSingleton<SettingsWindowViewModel>();
                services.AddTransient<SettingsWindow>();
                services.AddSingleton<MainWindowViewModel>();
            })
            .Build();
      
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await HostContainer.StartAsync();

        var mainWindow = new MainWindow();
        mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.DataContext = HostContainer.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Width = SystemParameters.PrimaryScreenWidth * 0.75;
        mainWindow.Height = SystemParameters.PrimaryScreenHeight * 0.75;
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        HostContainer.Services.GetRequiredService<Notifier>().Dispose();
        await HostContainer.StopAsync();
        base.OnExit(e);
    }
}



