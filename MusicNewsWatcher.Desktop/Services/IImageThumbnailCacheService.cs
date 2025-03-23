using Microsoft.Extensions.Options;
using MusicNewsWatcher.Core.Extensions;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using System.IO;
using System.Net;

namespace MusicNewsWatcher.Desktop.Services;

public interface IImageThumbnailCacheService
{
    string GetCachedImage(string originalSourceUri);
}

public class ImageThumbnailCacheServiceOptions
{
    public string PlaceholderFilePath { get; set; }
}

public class ImageThumbnailCacheService : IImageThumbnailCacheService
{
    private readonly string placeholderFilePath;

    public ImageThumbnailCacheService(IOptions<ImageThumbnailCacheServiceOptions> options)
    {
        this.placeholderFilePath = options.Value.PlaceholderFilePath;
    }

    public string GetCachedImage(string originalSourceUri)
    {
        string[] VALID_EXT = new[] { ".jpg", ".png", ".jpeg" };

        if (string.IsNullOrEmpty(originalSourceUri)) { return placeholderFilePath; }
        if (!originalSourceUri.StartsWith("http")) { return placeholderFilePath; }
        if (!Uri.IsWellFormedUriString(originalSourceUri, UriKind.Absolute)) { return placeholderFilePath; }

        string clearName = new(originalSourceUri.Except(Path.GetInvalidFileNameChars()).ToArray());
        string ext = Path.GetExtension(clearName);

        if (!VALID_EXT.Contains(ext.ToLower())) { ext = VALID_EXT[0]; }

        string cachedName = Path.Combine(FileBrowserHelper.CacheDirectory, originalSourceUri.GetMD5() + ext);
        if (!File.Exists(cachedName))
        {
            try
            {
                new WebClient().DownloadFile(originalSourceUri, cachedName);
            }
            catch (Exception ex)
            {
                File.Copy(placeholderFilePath, cachedName);
                return placeholderFilePath;
            }
        }
        return cachedName;
    }
}