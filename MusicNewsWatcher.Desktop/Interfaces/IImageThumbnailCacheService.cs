namespace MusicNewsWatcher.Desktop.Services;

public interface IImageThumbnailCacheService
{
    Task<string> GetCachedImage(string originalSourceUri, ThumbnailSize size);
}

public enum ThumbnailSize
{
    Artist,
    Album
}