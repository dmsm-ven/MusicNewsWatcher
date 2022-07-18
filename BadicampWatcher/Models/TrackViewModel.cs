namespace MusicNewsWatcher.ViewModels;

public class TrackViewModel : ViewModelBase
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string? DownloadUri { get; set; }
}