namespace MusicNewsWatcher.API.DataAccess.Entity;

public class TrackDownloadHistoryEntity
{
    public int Id { get; set; }
    public int TrackId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public int FileSizeInBytes { get; set; }
    public TrackEntity? Track { get; set; }
}