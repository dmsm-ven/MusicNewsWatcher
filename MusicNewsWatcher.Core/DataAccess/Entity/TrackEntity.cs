using MusicNewsWatcher.Core.DataAccess.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core;

[Table(name: "tracks")]
public class TrackEntity
{
    [Key]
    [Column(name: "id")]
    public int Id { get; set; }

    [Column(name: "albumid")]
    public int AlbumId { get; set; }

    [Column(name: "name")]
    public string Name { get; set; }

    [Column(name: "downloaduri")]
    public string? DownloadUri { get; set; }

    public AlbumEntity Album { get; set; }
}
