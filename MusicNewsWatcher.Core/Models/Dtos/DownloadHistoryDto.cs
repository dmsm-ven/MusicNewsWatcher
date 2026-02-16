namespace MusicNewsWatcher.Core.Models.Dtos;

public record TrackDownloadHistoryRequest(int TrackId, DateTime Started, DateTime Finished, int FileSizeInBytes);
public record TrackDownloadHistoryDto(string ArtistName, string AlbumName, string TrackName,
    DateTime Started,
    DateTime Finished,
    int FileSizeInBytes);

