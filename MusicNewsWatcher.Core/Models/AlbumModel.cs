namespace MusicNewsWatcher.Core.Models;
public class AlbumModel
{
    public string ArtistDisplayName { get; init; }
    public string AlbumDisplayName { get; init; }
    public IReadOnlyList<TrackModel> Tracks { get; init; }
}
