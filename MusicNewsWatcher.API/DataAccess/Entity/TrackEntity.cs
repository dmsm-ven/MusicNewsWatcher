namespace MusicNewsWatcher.API.DataAccess.Entity;

public class TrackEntity
{
    public int Id { get; set; }

    public int AlbumId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? DownloadUri { get; set; }

    public AlbumEntity? Album { get; set; }
    public List<TrackDownloadHistoryEntity> DownloadHistory { get; set; } = new();
}