using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core.DataAccess.Entity;

[Table(name: "tracks")]
public class TrackEntity
{
    [Key]
    [Column(name: "id")]
    public int Id { get; set; }

    [Column(name: "albumid")]
    public int AlbumId { get; set; }

    [Column(name: "name")]
    public string Name { get; set; } = string.Empty;

    [Column(name: "downloaduri")]
    public string? DownloadUri { get; set; }

    public AlbumEntity? Album { get; set; }
}
