using System;

namespace BandcampWatcher.DataAccess;

public class Album
{
    public int AlbumId { get; set; }
    public int ArtistId { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; }
    public bool IsViewed { get; set; }
    public string? Image { get; set; }
    public string Uri { get; set; }
}
