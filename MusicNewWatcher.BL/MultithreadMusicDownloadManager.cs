using MusicNewsWatcher.Core;
using System.Net;

namespace MusicNewWatcher.BL;

//TODO убрать прямое обращение через ViewModel
public class MultithreadHttpMusicDownloadManager : IMusicDownloadManager
{
    private readonly HttpClient client;
    private SemaphoreSlim? semaphor;

    private int threadLimit;
    public int ThreadLimit
    {
        get => threadLimit;
        set
        {
            if (threadLimit != value)
            {
                if (value >= 1 && value <= 8)
                {
                    threadLimit = value;
                    semaphor?.Dispose();
                    semaphor = new SemaphoreSlim(ThreadLimit);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(ThreadLimit), "must be in range 1..8");
                }
            }
        }
    }

    public MultithreadHttpMusicDownloadManager()
    {
        client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            CookieContainer = new CookieContainer(),
            UseCookies = true,
        });
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Accept", "*/*");
    }
    /*
    public async Task<string> DownloadFullAlbum(AlbumViewModel album, string downloadDirectory, CancellationToken token)
    {
        string albumDirectory = GetAlbumLocalPath(album, downloadDirectory);
        IList<TrackViewModel> source = album.Tracks;
        source.ToList().ForEach(i => i.DownloadResult = TrackDownloadResult.None);

        var tasks = album.Tracks.Select(track => CreateDownloadTrackTask(track, albumDirectory, token));

        await Task.WhenAll(tasks);

        return albumDirectory;
    }

    private async Task CreateDownloadTrackTask(TrackViewModel track, string albumDirectory, CancellationToken token)
    {
        await semaphor.WaitAsync(token);

        track.IsDownloading = true;

        string localName = Path.Combine(albumDirectory, Path.GetFileName(track.DownloadUri));

        TrackDownloadResult downloadResult = await DownloadTrack(track, localName, token);

        if (downloadResult == TrackDownloadResult.Success)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        track.IsDownloading = false;
        track.DownloadResult = downloadResult;

        semaphor.Release();

    }

    private async Task<TrackDownloadResult> DownloadTrack(TrackViewModel track, string localName, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(track.DownloadUri))
        {
            return TrackDownloadResult.Error;
        }

        if (File.Exists(localName))
        {
            if (new FileInfo(localName).Length == 0)
            {
                File.Delete(localName);
            }
            else
            {
                // пропускаем, файл уже скачан
                return TrackDownloadResult.Skipped;
            }
        }

        try
        {
            var bytes = await client.GetByteArrayAsync(track.DownloadUri, token);
            await File.WriteAllBytesAsync(localName, bytes);
            return TrackDownloadResult.Success;
        }
        catch (TaskCanceledException)
        {
            if (File.Exists(localName))
            {
                File.Delete(localName);
            }
            return TrackDownloadResult.Cancelled;
        }
        catch (Exception ex)
        {
            return TrackDownloadResult.Error;
        }

        return TrackDownloadResult.Error;
    }

    private string GetAlbumLocalPath(AlbumViewModel album, string downloadDirectory)
    {
        var directoryPath = Path.Combine(downloadDirectory,
            album.ParentArtist.DisplayName.RemoveInvalidCharacters(),
            album.DisplayName.RemoveInvalidCharacters());

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return directoryPath;
    }

    public void Dispose()
    {
        client?.Dispose();
        semaphor?.Dispose();
    }
    */

    public Task<string> DownloadFullAlbum(string albumUri, string downloadDirectory, CancellationToken? token = null)
    {
        throw new NotImplementedException();
    }
}


