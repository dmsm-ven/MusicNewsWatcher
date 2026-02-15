namespace MusicNewsWatcher.Core.Models.Dtos;

public record TrackDownloadHistoryRequest(int trackId, DateTime Started, DateTime Finished, int FileSizeInKb);
public record TrackDownloadHistoryDto(TrackDto track, DateTime Started, DateTime Finished, int FileSizeInKb);

