using ImageProcessor;
using ImageProcessor.Imaging;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.Core.Extensions;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.Services;

public interface IImageThumbnailCacheService
{
    Task<string> GetCachedImage(string originalSourceUri, ThumbnailSize size);
}

public class ImageThumbnailCacheServiceOptions
{
    public string PlaceholderFilePath { get; set; }
}

public class ImageThumbnailCacheService : IImageThumbnailCacheService
{
    private readonly string placeholderFilePath;
    private readonly IReadOnlyDictionary<ThumbnailSize, System.Drawing.Size> thumbnailSizes
        = new Dictionary<ThumbnailSize, System.Drawing.Size>()
        {
            [ThumbnailSize.Album] = new System.Drawing.Size(250, 250),
            [ThumbnailSize.Artist] = new System.Drawing.Size(500, 500),
        };
    private readonly HttpClient client;
    private readonly SemaphoreSlim semaphore = new(1, 1);

    public ImageThumbnailCacheService(IOptions<ImageThumbnailCacheServiceOptions> options, System.Net.Http.HttpClient client)
    {
        this.placeholderFilePath = options.Value.PlaceholderFilePath;
        this.client = client;
    }

    public async Task<string> GetCachedImage(string originalSourceUri, ThumbnailSize size)
    {
        string[] VALID_EXT = new[] { ".jpg", ".png", ".jpeg" };

        if (string.IsNullOrEmpty(originalSourceUri)) { return placeholderFilePath; }
        if (!originalSourceUri.StartsWith("http")) { return placeholderFilePath; }
        if (!Uri.IsWellFormedUriString(originalSourceUri, UriKind.Absolute)) { return placeholderFilePath; }

        string clearName = new(originalSourceUri.Except(Path.GetInvalidFileNameChars()).ToArray());
        string ext = Path.GetExtension(clearName);

        if (!VALID_EXT.Contains(ext.ToLower())) { ext = VALID_EXT[0]; }

        string cachedName = Path.Combine(FileBrowserHelper.CacheDirectory, size.ToString().ToLower(), originalSourceUri.GetMD5().ToLower() + ext);


        if (!File.Exists(cachedName))
        {
            await DownloadImageAndResize(originalSourceUri, cachedName, size);
        }

        return cachedName;
    }

    private async Task DownloadImageAndResize(string originalSource, string cachePath, ThumbnailSize size)
    {
        await semaphore.WaitAsync();

        string cacheDir = Path.GetDirectoryName(cachePath);

        if (!Directory.Exists(cacheDir))
        {
            Directory.CreateDirectory(cacheDir);
        }

        try
        {
            await using (var stream = await client.GetStreamAsync(originalSource))
            {
                await using var file = File.Create(cachePath);
                await stream.CopyToAsync(file);
            }

            await ResizeImage(cachePath, size);
        }
        catch
        {
            File.Copy(placeholderFilePath, cachePath, overwrite: true);
        }

        semaphore.Release();
    }

    private async Task ResizeImage(string image, ThumbnailSize sizeModel)
    {
        ResizeLayer resizeLayer = new(thumbnailSizes[sizeModel], ImageProcessor.Imaging.ResizeMode.Stretch);

        var tempFile = Path.GetTempFileName();

        using (var fs = new FileStream(image, FileMode.Open, FileAccess.Read))
        {
            using ImageFactory imageFactory = new(false);
            using var newFile = new FileStream(tempFile, FileMode.Create);

            imageFactory
            .Load(fs)
            .Resize(resizeLayer)
            .BackgroundColor(Color.White)
            .Save(newFile);
        }


        File.Delete(image);
        File.Move(tempFile, image);
    }
}

public enum ThumbnailSize
{
    Artist,
    Album
}