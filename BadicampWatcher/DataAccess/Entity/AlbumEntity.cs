using System;
using System.ComponentModel.DataAnnotations;

namespace BandcampWatcher.DataAccess;

public class AlbumEntity
{
    [Key]
    public int AlbumId { get; set; }

    public int ArtistId { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; }
    public bool IsViewed { get; set; }
    public string? Image { get; set; }
    public string Uri { get; set; }

    public ArtistEntity Artist { get; set; }
}
