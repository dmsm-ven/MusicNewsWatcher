using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewsWatcher.Models;
using MusicNewsWatcher.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using MusicNewsWatcher.Services;
using ToastNotifications;
using ToastNotifications.Messages;
using System.Diagnostics;
using System.Web;
using System.Threading;

namespace MusicNewsWatcher.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly MusicUpdateManager manager;
    private readonly MusicProviderBase provider;

    public event Action<AlbumViewModel> OnAlbumChanged;

    string title;
    public string Title
    {
        get => title;
        set
        {
            if(Set(ref title, value))
            {
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
    }
    public string DisplayName => Title.ToDisplayName();

    string cachedImage;
    public string CachedImage
    {
        get => cachedImage ??= GetCachedImage(Image);
    }

    public DateTime Created { get; init; }
    public int AlbumId { get; init; }
    public string ParentArtistName { get; init; }

    bool isActiveAlbum;
    public bool IsActiveAlbum
    {
        get => isActiveAlbum;
        set => Set(ref isActiveAlbum, value);
    }

    bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set
        {
            if(Set(ref inProgress, value))
            {
                RaisePropertyChanged(nameof(IsUpdateTracksButtonVisibile));
                RaisePropertyChanged(nameof(Tracks));
            }
        }
    }

    public bool IsUpdateTracksButtonVisibile
    {
        get => Tracks.Count == 0 && !InProgress;
    }

    public string? Image { get; set; }
    public string Uri { get; set; }

    public ObservableCollection<TrackViewModel> Tracks { get; }

    public ICommand RefreshTracksCommand { get; }
    public ICommand OnChangedCommand { get; }
    public ICommand DownloadAlbumCommand { get; }
    public ICommand CancelDownloadingCommand { get; }

    CancellationTokenSource cts;
    
    public AlbumViewModel()
    {
        CancelDownloadingCommand = new LambdaCommand(CancelDownloading, e => InProgress);
        DownloadAlbumCommand = new LambdaCommand(async e => await DownloadAlbum(), e => !InProgress);
        RefreshTracksCommand = new LambdaCommand(async e => await RefreshTracks());
        OnChangedCommand = new LambdaCommand(async e => await AlbumChanged());
        Tracks = new();
    }

    private void CancelDownloading(object obj)
    {
        App.HostContainer.Services.GetRequiredService<Notifier>().ShowError("Загрузка альбома отменена ...");
        cts.Cancel();
       
    }

    public AlbumViewModel(IDbContextFactory<MusicWatcherDbContext> dbFactory) : this()
    {
        this.dbFactory = dbFactory;
    }

    private async Task DownloadAlbum()
    {
        InProgress = true;
        cts = new CancellationTokenSource();

        MusicDownloadManager downloadManager = App.HostContainer.Services.GetRequiredService<MusicDownloadManager>();
        Notifier toasts = App.HostContainer.Services.GetRequiredService<Notifier>();
        Stopwatch sw = Stopwatch.StartNew();
        
        Action<TrackViewModel> indicator = (e) =>
        {
            e.IsDownloading = !e.IsDownloading;
            if (!e.IsDownloading)
            {
                toasts.ShowInformation($"'[{DateTime.Now.ToShortTimeString()}] Трек '{e.Name}' загружен");
            }
        };

        string downloadedFilesDirectory = await downloadManager.DownloadFullAlbum(this, indicator, cts.Token);

        InProgress = false;

        if (!cts.IsCancellationRequested)
        {
            string msg = $"[{DateTime.Now.ToShortTimeString()}] Альбом '{this.DisplayName}' загружен за {sw.Elapsed.ToString("hh\\:mm\\:ss")}";
            toasts.ShowSuccess(msg);
        }

        await Task.Delay(TimeSpan.FromSeconds(2));

        Process.Start("explorer.exe", downloadedFilesDirectory);
    }

    private async Task RefreshTracks()
    {
        InProgress = true;
        try
        {
            await manager.CheckUpdatesForAlbumAsync(provider, AlbumId);
            await RefreshTracksSource();
        }
        finally
        {
            InProgress = false;
        }
    }

    private async Task AlbumChanged()
    {
        if (!IsActiveAlbum)
        {
            OnAlbumChanged?.Invoke(this);
            IsActiveAlbum = true;

            InProgress = true;
            try
            {
                await RefreshTracksSource();
            }
            finally
            {
                InProgress = false;
            }
        }
    }

    private async Task RefreshTracksSource()
    {
        if (Tracks.Count > 0) { return; } 

        using var db = await dbFactory.CreateDbContextAsync();

        var tracks = db
            .Tracks.Where(a => a.AlbumId == AlbumId)
            .Select(i => new TrackViewModel()
            {
                AlbumId = i.AlbumId,
                Id = i.Id,
                Name = i.Name,
                DownloadUri = i.DownloadUri,
            })
            .OrderBy(a => a.Id)
            .ToList();

        foreach (var track in tracks)
        {
            Tracks.Add(track);
        }
    }

}
