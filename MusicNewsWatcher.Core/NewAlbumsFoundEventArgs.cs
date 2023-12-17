using MusicNewsWatcher.Core.Dto;

namespace MusicNewsWatcher.Core;

public class NewAlbumsFoundEventArgs
{
    public string Provider { get; init; } = string.Empty;
    public ArtistDto? Artist { get; init; }
    public AlbumDto[]? NewAlbums { get; init; }
}
