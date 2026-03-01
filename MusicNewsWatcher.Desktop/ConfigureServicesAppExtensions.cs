using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewsWatcher.Desktop.ViewModels.Windows;

namespace MusicNewsWatcher.Desktop;

public static class ConfigureServicesAppExtensions
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();


        // как вариант можно заменить все ViewModelFactory на Func версию, без вызова дополнительного метода Initialize
        services.AddTransient<MusicProviderViewModel>();
        services.AddSingleton<Func<MusicProviderDto, MusicProviderViewModel>>(sp => dto =>
        {
            return ActivatorUtilities.CreateInstance<MusicProviderViewModel>(sp, dto);
        });

        services.AddTransient<ArtistViewModel>();
        services.AddSingleton<Func<MusicProviderViewModel, ArtistDto, ArtistViewModel>>(sp => (parent, dto) =>
        {
            return ActivatorUtilities.CreateInstance<ArtistViewModel>(sp, parent, dto);
        });

        services.AddTransient<AlbumViewModel>();
        services.AddSingleton<Func<ArtistViewModel, AlbumDto, AlbumViewModel>>(sp => (parent, dto) =>
        {
            return ActivatorUtilities.CreateInstance<AlbumViewModel>(sp, parent, dto);
        });
    }

    public static void AddNotifyIcon(this IServiceCollection services)
    {
        /*var tbi = new TaskbarIcon();

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
        */
    }

    public static void AddToasts(this IServiceCollection services)
    {
        services.AddSingleton<IToastsNotifier, MockToastsNotifier>();
    }
}



