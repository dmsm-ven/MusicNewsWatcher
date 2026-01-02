using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.Core.Models;

public class NewAlbumFoundResult
{
    public string ProviderName { get; init; } = string.Empty;
    public string ArtistName { get; init; } = string.Empty;
    public string ArtistUri { get; init; } = string.Empty;
    public IReadOnlyList<AlbumDto> Albums { get; init; } = new List<AlbumDto>();
}