using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Services;
using BandcampWatcher.ViewModels;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace BandcampWatcher;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    readonly Mutex _mutex = new Mutex(false, "BandcampWatcherWpfApp");
    TaskbarIcon tbi;

    ISettingsStorage settingsStorage;

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!_mutex.WaitOne(500, false))
        {
            Application.Current.Shutdown();
        }

        

        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>();

                services.AddSingleton<ISettingsStorage>(x => new JsonSettingsStorage("app_settings.json"));
                services.AddSingleton<MusicProviderBase, BandcampMusicProvider>();
                services.AddSingleton<MusicProviderBase, MusifyMusicProvider>();
                services.AddSingleton<MusicManager>();
                services.AddSingleton<MainWindowViewModel>();
            })
            .Build();

        settingsStorage = host.Services.GetRequiredService<ISettingsStorage>();
        SettingsRoot settingsRoot = settingsStorage.Load();
        var mainWindow = new MainWindow();
        mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.Top = settingsRoot.MainWindowRectangle.Y;
        mainWindow.Left = settingsRoot.MainWindowRectangle.X;
        mainWindow.Width = settingsRoot.MainWindowRectangle.Width;
        mainWindow.Height = settingsRoot.MainWindowRectangle.Height;
        mainWindow.DataContext = host.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Show();

        ConfigureNotifyIcon();
    }

    private void ConfigureNotifyIcon()
    {
        if (tbi == null)
        {
            var tbiMenu = new ContextMenu();
            var showMenuItem = new MenuItem() { Header = "Отобразить" };
            showMenuItem.Click += (o, e) => Application.Current.MainWindow.Show();
            var exitMenuItem = new MenuItem() { Header = "Выход" };
            exitMenuItem.Click += (o, e) => Application.Current.Shutdown();

            tbiMenu.Items.Add(showMenuItem);
            tbiMenu.Items.Add(new Separator()); // null = separator
            tbiMenu.Items.Add(exitMenuItem);

            tbi = new TaskbarIcon();
            tbi.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/logo.ico")).Stream);
            tbi.ToolTipText = "Bandcamp парсер музыкальных новинок";
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
}



