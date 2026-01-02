namespace MusicNewsWatcher.Core.Models;

public class AlbumDownloadModel
{
    public string ArtistDisplayName { get; init; } = string.Empty;
    public string AlbumDisplayName { get; init; } = string.Empty;
    public IReadOnlyList<TrackDownloadModel>? Tracks { get; init; } = null;
}
