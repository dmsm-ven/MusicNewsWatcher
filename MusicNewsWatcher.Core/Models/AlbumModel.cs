namespace MusicNewsWatcher.Core.Models;

public class AlbumModel
{
    public string ArtistDisplayName { get; init; } = string.Empty;
    public string AlbumDisplayName { get; init; } = string.Empty;
    public IReadOnlyList<TrackModel>? Tracks { get; init; } = null;
}
