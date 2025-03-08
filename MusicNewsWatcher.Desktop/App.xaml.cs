global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using MusicNewsWatcher.Core;
global using MusicNewsWatcher.Desktop.Services;
global using MusicNewsWatcher.Desktop.ViewModels;
global using MusicNewsWatcher.Desktop.Views;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using ToastNotifications;
global using ToastNotifications.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Core.Models;
using MusicNewWatcher.BL;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
                //options.AddUserSecrets(this.GetType().Assembly);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContextFactory<MusicWatcherDbContext>(options =>
                {
                    options.UseNpgsql(context.Configuration.GetConnectionString("default"));
                });

                services.AddToasts();
                //services.AddNotifyIcon();
                services.AddTelegramBot(context.Configuration);

                services.AddTransient<ISyncLibraryTracker, SyncLibraryTracker>();
                services.AddTransient<IDialogWindowService, DialogWindowService>();

                services.AddTransient<AddNewArtistDialogViewModel>();
                services.AddTransient<AddNewArtistDialog>();

                services.AddMusicProviders();
                services.AddSingleton<IMusicDownloadManager, SimpleHttpMusicDownloadManager>();
                services.AddSingleton<IMusicNewsCrawler, EfMusicNewsCrawler>();
                services.AddSingleton<MusicDownloadHelper>();
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
        //mainWindow.Closing += (o, e) => { e.Cancel = true; mainWindow.Hide(); };
        mainWindow.DataContext = HostContainer.Services.GetRequiredService<MainWindowViewModel>();
        mainWindow.Width = SystemParameters.PrimaryScreenWidth * sizeRatio;
        mainWindow.Height = SystemParameters.PrimaryScreenHeight * sizeRatio;

        //await DownloadCustomAlbums();

        mainWindow.ShowDialog();
    }

    private async Task DownloadCustomAlbums()
    {
        var albumsToDownload = File.ReadAllLines(@"C:\Users\user\Desktop\music-to-download.txt");
        var downloadFolder = @"D:\Programming\Projects\Parsing\MusicNewsWatcher\MusicNewsWatcher.Desktop\bin\Debug\net6.0-windows\downloads";


        MusicUpdateManager updateManager = HostContainer.Services.GetRequiredService<MusicUpdateManager>();
        MusicProviderBase musifyProvider = HostContainer.Services.GetRequiredService<IEnumerable<MusicProviderBase>>().Last();
        IMusicDownloadManager downloadManager = HostContainer.Services.GetRequiredService<IMusicDownloadManager>();

        int i = 0;
        foreach (var uri in albumsToDownload)
        {
            var albumItem = new Core.DataAccess.Entity.AlbumEntity()
            {
                Uri = uri,
                AlbumId = i++,
            };
            var albumTracks = await musifyProvider.GetTracksAsync(albumItem);

            var albumModel = new AlbumModel()
            {
                Tracks = albumTracks.Select(i => new TrackModel() { DownloadUri = i.DownloadUri }).ToList(),
                AlbumDisplayName = $"Album_number_{albumItem.AlbumId}",
                ArtistDisplayName = "CustomArtist"
            };

            await downloadManager.DownloadFullAlbum(albumModel, downloadFolder);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await HostContainer.StopAsync();
        mutex?.ReleaseMutex();

        base.OnExit(e);
    }
}



