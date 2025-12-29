namespace MusicNewsWatcher.Core.Models.Dtos;

public record MusicProviderDto(
    int MusicProviderId,
    string Name,
    string Image,
    string Uri,
    int TotalArtists
);

