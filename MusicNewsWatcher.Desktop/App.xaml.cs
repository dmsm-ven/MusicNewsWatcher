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
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.ViewModels.Windows;
using System.Net.Http;
using System.Windows;

namespace MusicNewsWatcher.Desktop;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost HostContainer { get; private set; }
    public App()
    {
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
                services.AddOptions<ApiConnectionConfiguration>().Bind(context.Configuration.GetSection(nameof(ApiConnectionConfiguration)));

                services.AddHttpClient<MultithreadHttpDownloadManager>();
                services.AddHttpClient(nameof(MusicNewsWatcherApiClient), client =>
                {
                    var options = context.Configuration.GetSection(nameof(ApiConnectionConfiguration)).Get<ApiConnectionConfiguration>()
 ?? throw new InvalidOperationException("MusicWatcherApiConfiguration is not configured properly.");
                    client.BaseAddress = new Uri(options.Host);
                    client.DefaultRequestHeaders.Authorization = new("Bearer", options.AccessToken);
                });
                services.AddSingleton<MusicNewsWatcherApiClient>(sp =>
                {
                    var factory = sp.GetRequiredService<IHttpClientFactory>();
                    var httpClient = factory.CreateClient(nameof(MusicNewsWatcherApiClient));
                    return new MusicNewsWatcherApiClient(httpClient);
                });
                services.AddSingleton<IImageThumbnailCacheService, ImageThumbnailCacheService>();
                services.AddTransient<IDialogWindowService, DialogWindowService>();

                services.AddTransient<ArtistAddWindow>();
                services.AddTransient<ArtistAddWindowViewModel>();

                services.AddTransient<DownloadHistoryWindowViewModel>();
                services.AddSingleton<MusicDownloadHelper>();

                services.AddViewModels();
            })
            .Build();
    }
    protected override async void OnStartup(StartupEventArgs e)
    {
        var apiClient = HostContainer.Services.GetRequiredService<MusicNewsWatcherApiClient>();
        if ((await apiClient.CheckApiStatusAsync()) == false)
        {
            MessageBox.Show("Нет соединения с сервером", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

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
        base.OnExit(e);
    }
}



