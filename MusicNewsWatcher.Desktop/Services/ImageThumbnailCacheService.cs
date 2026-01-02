using Microsoft.Extensions.Options;
using MusicNewsWatcher.Desktop.Extensions;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.IO;
using System.Net.Http;

namespace MusicNewsWatcher.Desktop.Services;

public class ImageThumbnailCacheService : IImageThumbnailCacheService
{
    private readonly string placeholderFilePath;
    private readonly IReadOnlyDictionary<ThumbnailSize, Size> thumbnailSizes;
    private readonly HttpClient client;
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private static readonly JpegEncoder JPEG_ENCODER = new JpegEncoder() { Quality = 90 };

    public ImageThumbnailCacheService(IOptions<ImageThumbnailCacheServiceOptions> options, HttpClient client)
    {
        this.placeholderFilePath = options.Value.PlaceholderFilePath;
        this.client = client;

        thumbnailSizes = new Dictionary<ThumbnailSize, Size>()
        {
            [ThumbnailSize.Album] = new(options.Value.AlbumThumbinailSize, options.Value.AlbumThumbinailSize),
            [ThumbnailSize.Artist] = new(options.Value.ArtistThumbnailSize, options.Value.ArtistThumbnailSize),
        };
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

        var img = SixLabors.ImageSharp.Image.Load(image);

        img.Mutate(x =>
        {
            x.Resize(new SixLabors.ImageSharp.Size(thumbnailSizes[sizeModel].Width, thumbnailSizes[sizeModel].Height));
            x.BackgroundColor(SixLabors.ImageSharp.Color.White);
        });

        string tmpPath = Path.GetTempFileName();
        using (var newFile = new FileStream(tmpPath, FileMode.Create))
        {
            await img.SaveAsync(newFile, JPEG_ENCODER);
        }

        File.Delete(image);
        File.Move(tmpPath, image);
    }
}
