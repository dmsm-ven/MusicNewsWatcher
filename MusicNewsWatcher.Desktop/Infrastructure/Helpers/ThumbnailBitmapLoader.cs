using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace MusicNewsWatcher.Desktop.Infrastructure.Helpers;

/// <summary>
/// Загружает превью с диска в уменьшенном до размера карточки виде и держит небольшой LRU кэш
/// уже декодированных (замороженных) изображений, чтобы прокрутка списка туда-обратно не читала диск повторно.
/// Методы нужно вызывать только из UI потока (кэш не потокобезопасен намеренно).
/// </summary>
public static class ThumbnailBitmapLoader
{
    private const int Capacity = 200;

    private static readonly Dictionary<string, LinkedListNode<CacheEntry>> map = new();
    private static readonly LinkedList<CacheEntry> lru = new();

    private sealed record CacheEntry(string Key, BitmapSource Bitmap);

    public static async Task<BitmapSource?> LoadAsync(string path, int decodeWidth)
    {
        string key = $"{path}|{decodeWidth}";

        if (map.TryGetValue(key, out var cachedNode))
        {
            lru.Remove(cachedNode);
            lru.AddFirst(cachedNode);
            return cachedNode.Value.Bitmap;
        }

        BitmapSource? bitmap = await Task.Run(() => Decode(path, decodeWidth));
        if (bitmap == null)
        {
            return null;
        }

        // Продолжение выполняется снова в UI потоке
        if (map.TryGetValue(key, out var existingNode))
        {
            return existingNode.Value.Bitmap;
        }

        map[key] = lru.AddFirst(new CacheEntry(key, bitmap));

        if (map.Count > Capacity && lru.Last != null)
        {
            string oldestKey = lru.Last.Value.Key;
            lru.RemoveLast();
            map.Remove(oldestKey);
        }

        return bitmap;
    }

    private static BitmapSource? Decode(string path, int decodeWidth)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            image.DecodePixelWidth = decodeWidth;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Не удалось прочитать превью '{path}': {ex.Message}");
            return null;
        }
    }
}
