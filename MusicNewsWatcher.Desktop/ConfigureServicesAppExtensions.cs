using Hardcodet.Wpf.TaskbarNotification;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewsWatcher.Desktop.ViewModels.Windows;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace MusicNewsWatcher.Desktop;

public static class ConfigureServicesAppExtensions
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<SettingsWindowViewModel>();
        services.AddTransient<SettingsWindow>();

        services.AddTransient<SyncLibraryWindowViewModel>();
        services.AddTransient<SyncLibraryWindow>();
        services.AddTransient<MainWindowViewModel>();

        services.AddTransient<AlbumViewModel>();
        services.AddTransient<ViewModelFactory<AlbumViewModel>>();

        services.AddTransient<ArtistViewModel>();
        services.AddTransient<ViewModelFactory<ArtistViewModel>>();

        services.AddTransient<MusicProviderViewModel>();
        services.AddTransient<ViewModelFactory<MusicProviderViewModel>>();
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

    public static void AddToasts(this IServiceCollection services)
    {
        services.AddSingleton<IToastsNotifier, MockToastsNotifier>();
    }
}



