namespace MusicNewsWatcher.Core.Models.Dtos;

public record ArtistDto(int MusicProviderId, int ArtistId, string Name, string Uri, string Image);
public record CreateArtistDto(int MusicProviderId, string Name, string Uri, string Image);
