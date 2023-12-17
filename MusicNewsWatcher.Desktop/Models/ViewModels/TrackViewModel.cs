using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewsWatcher.Desktop.Services;
using MusicNewsWatcher.Desktop.Models.ViewModels;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class TrackViewModel : ViewModelBase
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Name { get; set; } = "<Без названия>";
    public string DisplayName => Name.ToDisplayName();
    public string? DownloadUri { get; set; }

    public bool IsDownloaded => DownloadResult != TrackDownloadResult.None;

    TrackDownloadResult downloadResult;
    public TrackDownloadResult DownloadResult
    {
        get => downloadResult;
        set
        {
            if(Set(ref downloadResult, value))
            {
                RaisePropertyChanged(nameof(IsDownloaded));
            }
        }
    }

    bool isDownloading;
    public bool IsDownloading
    {
        get => isDownloading;
        set => Set(ref isDownloading, value);
    }
    public AlbumViewModel Parent { get; }

    public TrackViewModel(AlbumViewModel parent)
    {
        Parent = parent;
    }
}