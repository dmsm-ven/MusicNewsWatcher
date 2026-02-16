namespace MusicNewsWatcher.Core.Models;

public class TrackDownloadModel
{
    public int TrackId { get; init; }
    public string? DownloadUri { get; init; } = string.Empty;
}