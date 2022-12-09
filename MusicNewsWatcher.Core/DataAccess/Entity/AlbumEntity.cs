using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core;

[Table(name: "albums")]
public class AlbumEntity
{
    [Key]
    [Column(name: "albumid")]
    public int AlbumId { get; set; }

    [Column(name: "artistid")]
    public int ArtistId { get; set; }

    [Column(name: "title")]
    public string Title { get; set; }

    [Column(name: "created")]
    public DateTime Created { get; set; } = DateTime.Now.Date;

    [Column(name: "isviewed")]
    public bool IsViewed { get; set; }

    [Column(name: "image")]
    public string? Image { get; set; }

    [Column(name: "uri")]
    public string Uri { get; set; }

    public ArtistEntity Artist { get; set; }
    public List<TrackEntity> Tracks { get; set; } = new List<TrackEntity>();
}
