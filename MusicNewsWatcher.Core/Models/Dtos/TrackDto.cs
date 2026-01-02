namespace MusicNewsWatcher.Core.Models.Dtos;

public record TrackDto(
    int Id,
    int AlbumId,
    string Name,
    string? DownloadUri
);

