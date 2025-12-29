global using Microsoft.Extensions.DependencyInjection;
global using MusicNewsWatcher.Desktop.Services;
global using MusicNewsWatcher.Desktop.ViewModels;
global using MusicNewsWatcher.Desktop.Views;
global using System;
global using System.Collections.Generic;
global using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.ViewModels.Windows;
using System.Windows;

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
                options.AddUserSecrets(this.GetType().Assembly);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddToasts();
                services.AddOptions<MusicDownloadFolderOptions>().Bind(context.Configuration.GetSection(nameof(MusicDownloadFolderOptions)));
                services.AddOptions<ImageThumbnailCacheServiceOptions>().Bind(context.Configuration.GetSection(nameof(ImageThumbnailCacheServiceOptions)));
                services.AddOptions<MusicWatcherApiConfiguration>().Bind(context.Configuration.GetSection(nameof(ImageThumbnailCacheServiceOptions)));
                services.AddHttpClient();
                services.AddHttpClient<MusicNWatcherApiClient>(client =>
                {
                    var options = context.Configuration.GetSection(nameof(MusicWatcherApiConfiguration)).Get<MusicWatcherApiConfiguration>()
                     ?? throw new InvalidOperationException("MusicWatcherApiConfiguration is not configured properly.");
                    client.BaseAddress = new Uri(options.HostUri);
                    client.DefaultRequestHeaders.Authorization = new("Bearer", options.AccessToken);
                });

                services.AddSingleton<IImageThumbnailCacheService, ImageThumbnailCacheService>();
                services.AddTransient<ISyncLibraryTracker, SyncLibraryTracker>();
                services.AddTransient<IDialogWindowService, DialogWindowService>();

                services.AddTransient<AddOrEditArtistDialog>();
                services.AddTransient<AddOrEditArtistDialogViewModel>();

                services.AddSingleton<MusicDownloadHelper>();

                services.AddViewModels();
            })
            .Build();
    }
    protected override async void OnStartup(StartupEventArgs e)
    {
        //Окно занимает 85% экрана
        double sizeRatio = 0.85;

        var mainWindow = new MainWindow();
        //mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.DataContext = HostContainer.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Width = SystemParameters.PrimaryScreenWidth * sizeRatio;
        mainWindow.Height = SystemParameters.PrimaryScreenHeight * sizeRatio;
        mainWindow.ShowDialog();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await HostContainer.StopAsync();
        mutex?.ReleaseMutex();

        base.OnExit(e);
    }
}



