using Microsoft.Extensions.Options;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicNewsWatcher.Desktop.Services;

/// <summary>
/// Скачивает обложки, уменьшает их до квадратного превью средствами WPF (без сторонних библиотек)
/// и хранит результат на диске в виде небольших JPEG файлов.
/// В памяти ничего не держит: возвращается только путь до файла.
/// </summary>
public sealed class ImageThumbnailCacheService : IImageThumbnailCacheService
{
    public const string HttpClientName = "thumbnails";

    private const long MaxDownloadBytes = 15 * 1024 * 1024;
    private const int JpegQuality = 85;
    private const int MaxParallelDownloads = 3;

    private static readonly TimeSpan CacheLifetime = TimeSpan.FromDays(90);
    private static readonly TimeSpan FailureRetryDelay = TimeSpan.FromMinutes(10);

    private readonly string placeholderFilePath;
    private readonly IReadOnlyDictionary<ThumbnailSize, int> thumbnailSizes;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly SemaphoreSlim downloadGate = new(MaxParallelDownloads, MaxParallelDownloads);

    // Один и тот же URL одновременно качается только один раз
    private readonly ConcurrentDictionary<string, Lazy<Task<string>>> inflight = new();

    // Неудачные загрузки не кэшируем на диск, но и не долбим сервер повторно каждую секунду
    private readonly ConcurrentDictionary<string, DateTime> failedUntil = new();

    private int cleanupStarted;

    public ImageThumbnailCacheService(IOptions<ImageThumbnailCacheServiceOptions> options, IHttpClientFactory httpClientFactory)
    {
        var settings = options.Value;

        this.httpClientFactory = httpClientFactory;
        this.placeholderFilePath = Path.IsPathRooted(settings.PlaceholderFilePath)
            ? settings.PlaceholderFilePath
            : Path.Combine(AppContext.BaseDirectory, settings.PlaceholderFilePath);

        this.thumbnailSizes = new Dictionary<ThumbnailSize, int>()
        {
            [ThumbnailSize.Album] = Math.Max(settings.AlbumThumbnailSize, 32),
            [ThumbnailSize.Artist] = Math.Max(settings.ArtistThumbnailSize, 32),
        };
    }

    public Task<string> GetCachedImage(string? originalSourceUri, ThumbnailSize size)
    {
        if (string.IsNullOrWhiteSpace(originalSourceUri) ||
            !Uri.TryCreate(originalSourceUri.Trim(), UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return Task.FromResult(placeholderFilePath);
        }

        StartCleanupOnce();

        string cachedPath = Path.Combine(
            FileBrowserHelper.CacheDirectory,
            size.ToString().ToLowerInvariant(),
            HashName(uri.AbsoluteUri) + ".jpg");

        if (File.Exists(cachedPath))
        {
            return Task.FromResult(cachedPath);
        }

        if (failedUntil.TryGetValue(cachedPath, out var retryAfter) && retryAfter > DateTime.UtcNow)
        {
            return Task.FromResult(placeholderFilePath);
        }

        // Task.Run: вся подготовительная работа (диск, сеть, декодирование) выполняется не в UI потоке
        return inflight
            .GetOrAdd(cachedPath, _ => new Lazy<Task<string>>(() => Task.Run(() => CreateThumbnailAsync(uri, cachedPath, size))))
            .Value;
    }

    private async Task<string> CreateThumbnailAsync(Uri uri, string cachedPath, ThumbnailSize size)
    {
        try
        {
            await downloadGate.WaitAsync().ConfigureAwait(false);
            try
            {
                if (File.Exists(cachedPath))
                {
                    return cachedPath;
                }

                Directory.CreateDirectory(Path.GetDirectoryName(cachedPath)!);

                using var source = await DownloadAsync(uri).ConfigureAwait(false);
                byte[] jpeg = BuildSquareJpeg(source, thumbnailSizes[size]);

                // Пишем во временный файл и переименовываем: недокачанный файл никогда не попадёт в кэш
                string tempPath = $"{cachedPath}.{Guid.NewGuid():N}.tmp";
                await File.WriteAllBytesAsync(tempPath, jpeg).ConfigureAwait(false);
                File.Move(tempPath, cachedPath, overwrite: true);

                failedUntil.TryRemove(cachedPath, out _);
                return cachedPath;
            }
            finally
            {
                downloadGate.Release();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Не удалось получить превью {uri}: {ex.Message}");
            failedUntil[cachedPath] = DateTime.UtcNow + FailureRetryDelay;
            return placeholderFilePath;
        }
        finally
        {
            inflight.TryRemove(cachedPath, out _);
        }
    }

    private async Task<MemoryStream> DownloadAsync(Uri uri)
    {
        var client = httpClientFactory.CreateClient(HttpClientName);

        using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength is > MaxDownloadBytes)
        {
            throw new InvalidOperationException("Изображение слишком большое");
        }

        await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        var buffer = new MemoryStream();
        try
        {
            var chunk = new byte[81920];
            int read;
            while ((read = await stream.ReadAsync(chunk).ConfigureAwait(false)) > 0)
            {
                if (buffer.Length + read > MaxDownloadBytes)
                {
                    throw new InvalidOperationException("Изображение слишком большое");
                }
                buffer.Write(chunk, 0, read);
            }

            buffer.Position = 0;
            return buffer;
        }
        catch
        {
            buffer.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Декодирует изображение сразу в уменьшенном виде (большой оригинал целиком в память не попадает),
    /// вырезает центральный квадрат, подкладывает белый фон под прозрачность и кодирует в JPEG.
    /// </summary>
    private static byte[] BuildSquareJpeg(Stream source, int targetSize)
    {
        source.Position = 0;
        var frame = BitmapFrame.Create(source, BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.None);
        int width = frame.PixelWidth;
        int height = frame.PixelHeight;

        if (width <= 0 || height <= 0)
        {
            throw new InvalidDataException("Пустое изображение");
        }

        source.Position = 0;
        var decoded = new BitmapImage();
        decoded.BeginInit();
        decoded.CacheOption = BitmapCacheOption.OnLoad;
        decoded.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
        decoded.StreamSource = source;

        // Уменьшаем так, чтобы КОРОТКАЯ сторона стала равна targetSize (пропорции сохраняются), увеличивать не нужно
        if (Math.Min(width, height) > targetSize)
        {
            if (width <= height)
            {
                decoded.DecodePixelWidth = targetSize;
            }
            else
            {
                decoded.DecodePixelHeight = targetSize;
            }
        }

        decoded.EndInit();
        decoded.Freeze();

        int side = Math.Min(decoded.PixelWidth, decoded.PixelHeight);
        var cropArea = new Int32Rect((decoded.PixelWidth - side) / 2, (decoded.PixelHeight - side) / 2, side, side);

        BitmapSource square = FlattenOnWhite(new CroppedBitmap(decoded, cropArea));

        var encoder = new JpegBitmapEncoder() { QualityLevel = JpegQuality };
        encoder.Frames.Add(BitmapFrame.Create(square));

        using var output = new MemoryStream();
        encoder.Save(output);
        return output.ToArray();
    }

    private static BitmapSource FlattenOnWhite(BitmapSource source)
    {
        var converted = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
        int width = converted.PixelWidth;
        int height = converted.PixelHeight;
        int stride = width * 4;

        var pixels = new byte[stride * height];
        converted.CopyPixels(pixels, stride, 0);

        for (int i = 0; i < pixels.Length; i += 4)
        {
            int alpha = pixels[i + 3];
            if (alpha != 255)
            {
                int inverse = 255 * (255 - alpha);
                pixels[i] = (byte)((pixels[i] * alpha + inverse) / 255);
                pixels[i + 1] = (byte)((pixels[i + 1] * alpha + inverse) / 255);
                pixels[i + 2] = (byte)((pixels[i + 2] * alpha + inverse) / 255);
            }
            pixels[i + 3] = 255;
        }

        var flattened = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
        flattened.Freeze();
        return flattened;
    }

    private static string HashName(string value)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
    }

    /// <summary>
    /// Один раз за запуск удаляет из кэша файлы старше CacheLifetime (и забытые .tmp)
    /// </summary>
    private void StartCleanupOnce()
    {
        if (Interlocked.Exchange(ref cleanupStarted, 1) == 1)
        {
            return;
        }

        _ = Task.Run(() =>
        {
            try
            {
                var threshold = DateTime.UtcNow - CacheLifetime;
                foreach (var file in Directory.EnumerateFiles(FileBrowserHelper.CacheDirectory, "*", SearchOption.AllDirectories))
                {
                    if (File.GetLastWriteTimeUtc(file) < threshold)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка очистки кэша превью: {ex.Message}");
            }
        });
    }
}
