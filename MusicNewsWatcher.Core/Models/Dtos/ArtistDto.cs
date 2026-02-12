namespace MusicNewsWatcher.Core.Models.Dtos;

public class ArtistDto
{
    public int MusicProviderId { get; init; }
    public int ArtistId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
}
public record CreateArtistDto(int MusicProviderId, string Name, string Uri, string Image);
