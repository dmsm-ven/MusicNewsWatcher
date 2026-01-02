namespace MusicNewsWatcher.Core.Models.Dtos;

public record AlbumDto(int AlbumId, int ArtistId, string Title, DateTime Created, bool IsViewed, string? Image, string Uri);