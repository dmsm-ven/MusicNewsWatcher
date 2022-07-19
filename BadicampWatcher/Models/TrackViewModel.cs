using MusicNewsWatcher.Infrastructure.Helpers;
using System.Web;

namespace MusicNewsWatcher.ViewModels;

public class TrackViewModel : ViewModelBase
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string DisplayName => Name.ToDisplayName();
    public string? DownloadUri { get; set; }


    bool isDownloaded;
    public bool IsDownloaded
    {
        get => isDownloaded;
        set => Set(ref isDownloaded, value);
    }

    bool isDownloading;
    public bool IsDownloading
    {
        get => isDownloading;
        set => Set(ref isDownloading, value);
    }
}