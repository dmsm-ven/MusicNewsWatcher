using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.Core.Models;

public class NewAlbumsFoundEventArgs
{
    public string Provider { get; init; } = string.Empty;
    public ArtistDto? Artist { get; init; } = null;
    public AlbumDto[] NewAlbums { get; init; } = Array.Empty<AlbumDto>();
}
