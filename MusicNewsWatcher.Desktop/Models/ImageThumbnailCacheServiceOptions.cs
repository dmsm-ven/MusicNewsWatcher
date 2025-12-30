namespace MusicNewsWatcher.Desktop.Services;

public class ImageThumbnailCacheServiceOptions
{
    public string PlaceholderFilePath { get; set; }
    public int ArtistThumbnailSize { get; set; } = 500;
    public int AlbumThumbinailSize { get; set; } = 250;
}
