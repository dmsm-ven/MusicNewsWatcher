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

namespace MusicNewsWatcher.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly MusicManager manager;
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
    public string DisplayName => Regex.Replace(Title, @"\s{2,}", " ").Trim();

    string cachedImage;
    public string CachedImage
    {
        get => cachedImage ??= GetCachedImage(Image);
    }

    public DateTime Created { get; init; }
    public int AlbumId { get; init; }

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

    public AlbumViewModel()
    {
        RefreshTracksCommand = new LambdaCommand(async e => await RefreshTracks());
        OnChangedCommand = new LambdaCommand(async e => await AlbumChanged());
        Tracks = new();
    }

    public AlbumViewModel(IDbContextFactory<MusicWatcherDbContext> dbFactory, MusicManager manager, MusicProviderBase provider) : this()
    {
        this.dbFactory = dbFactory;
        this.manager = manager;
        this.provider = provider;
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
