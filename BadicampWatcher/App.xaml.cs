using BandcampWatcher.Models;
using BandcampWatcher.Services;
using BandcampWatcher.ViewModels;
using Hardcodet.Wpf.TaskbarNotification;
using System;
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

    ISettingsStorage settingsStorage = new JsonSettingsStorage("app_settings.json");

    protected override void OnStartup(StartupEventArgs e)
    {
        if (!_mutex.WaitOne(500, false))
        {
            Application.Current.Shutdown();
        }

        var settingsRoot = settingsStorage.Load();

        var vm = new MainWindowViewModel(settingsStorage);

        var mainWindow = new MainWindow();
        mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.Top = settingsRoot.MainWindowRectangle.Y;
        mainWindow.Left = settingsRoot.MainWindowRectangle.X;
        mainWindow.Width = settingsRoot.MainWindowRectangle.Width;
        mainWindow.Height = settingsRoot.MainWindowRectangle.Height;
        mainWindow.DataContext = vm;
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



