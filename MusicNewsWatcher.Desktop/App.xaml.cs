global using Microsoft.Extensions.DependencyInjection;
global using MusicNewsWatcher.Desktop.Services;
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
using System.Net;
using System.Net.Http;
using System.Windows;

namespace MusicNewsWatcher.Desktop;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost? HostContainer { get; private set; }
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
                services.AddOptions<ApiConnectionConfiguration>().Bind(context.Configuration.GetSection(nameof(ApiConnectionConfiguration)));
                services.AddOptions<HttpClientParserConfiguration>().Bind(context.Configuration.GetSection(nameof(HttpClientParserConfiguration)));

                services.AddHttpClient<MultithreadHttpDownloadManager>(client =>
                {
                    var userAgent = context.Configuration.GetValue<string>("HttpClientParserConfiguration:UserAgent");
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,ru;q=0.8");
                }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    CookieContainer = new CookieContainer(),
                    UseCookies = true // This is true by default
                });

                // Клиент для загрузки обложек (используется ImageThumbnailCacheService)
                services.AddHttpClient(ImageThumbnailCacheService.HttpClientName, client =>
                {
                    var userAgent = context.Configuration.GetValue<string>("HttpClientParserConfiguration:UserAgent");
                    if (!string.IsNullOrWhiteSpace(userAgent))
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
                    }
                    // WebP/AVIF средствами WPF не прочитать, поэтому просим только jpeg/png
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "image/jpeg,image/png,image/*;q=0.5");
                    client.Timeout = TimeSpan.FromSeconds(30);
                });

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
        try
        {
            var apiClient = HostContainer!.Services.GetRequiredService<MusicNewsWatcherApiClient>();
            if ((await apiClient.CheckApiStatusAsync()) == false)
            {
                MessageBox.Show("Нет соединения с сервером", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
                return;
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
        catch (Exception ex)
        {
            // async void: необработанное исключение здесь уронило бы приложение без объяснений
            MessageBox.Show($"Ошибка запуска приложения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Host только собирался (Build) и не запускался, поэтому StopAsync не нужен, достаточно освободить ресурсы
        HostContainer?.Dispose();
        base.OnExit(e);
    }
}



