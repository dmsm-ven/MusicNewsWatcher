using CommunityToolkit.Mvvm.ComponentModel;
using MusicNewsWatcher.Core.Models;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

public partial class TrackViewModel : ObservableObject
{
    public int Id { get; init; }
    public int AlbumId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? DownloadUri { get; init; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDownloaded))]
    private TrackDownloadResult downloadResult;

    [ObservableProperty]
    private bool isDownloading;

    public AlbumViewModel Parent { get; }

    public bool IsDownloaded => DownloadResult != TrackDownloadResult.None;

    public TrackViewModel(AlbumViewModel parent)
    {
        Parent = parent;
    }
}