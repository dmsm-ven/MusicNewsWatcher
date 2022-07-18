using System.ComponentModel.DataAnnotations;

namespace MusicNewsWatcher.DataAccess;

public class TrackEntity
{
    [Key]
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string? DownloadUri { get; set; }

    public AlbumEntity Album { get; set; }
}
