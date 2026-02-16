using Microsoft.Extensions.Logging;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Desktop.Extensions;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace MusicNewsWatcher.Desktop.Services;

//TODO убрать прямое обращение через ViewModel
public class MultithreadHttpDownloadManager(HttpClient client,
        ILogger<MultithreadHttpDownloadManager> logger,
        MusicNewsWatcherApiClient apiClient)
{
    private SemaphoreSlim? semaphor = new(1);
    private readonly ConcurrentDictionary<TrackDownloadModel, TrackDownloadResult> downloadStatuses = new();

    private int threadLimit = 1;
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

    /// <summary>
    /// Загружает альбом по указанному URI и возращает путь до созданной папки в которую были загружены все треки альбома 
    /// </summary>
    /// <param name="albumUri"></param>
    /// <param name="downloadDirectory"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task DownloadFullAlbum(AlbumDownloadModel album, string downloadDirectory, CancellationToken? token = null)
    {
        logger.LogInformation("Начало загрузки альбома {albumName} в {threadLimit} потока(ов)", album.AlbumDisplayName, ThreadLimit);

        if (album.Tracks == null || album.Tracks.Count == 0)
        {
            throw new ArgumentException("Альбом не содержит треков для загрузки", nameof(album));
        }

        string albumDirectory = GetAlbumLocalPath(album, downloadDirectory);

        downloadStatuses.Clear();


        album.Tracks.ToList().ForEach(i => downloadStatuses[i] = TrackDownloadResult.None);

        var tasks = album.Tracks.Select(track => CreateDownloadTrackTask(track, albumDirectory, token));

        await Task.WhenAll(tasks);
    }
    private string GetAlbumLocalPath(AlbumDownloadModel album, string downloadDirectory)
    {
        var directoryPath = Path.Combine(downloadDirectory,
            album.ArtistDisplayName.RemoveInvalidCharacters(),
            album.AlbumDisplayName.RemoveInvalidCharacters());

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return directoryPath;
    }
    private async Task CreateDownloadTrackTask(TrackDownloadModel track, string albumDirectory, CancellationToken? token = null)
    {
        if (string.IsNullOrWhiteSpace(track.DownloadUri) || !Uri.IsWellFormedUriString(track.DownloadUri, UriKind.Absolute))
        {
            return;
        }

        await semaphor!.WaitAsync(token ?? CancellationToken.None);

        string rawName = Regex.Replace(track.DownloadUri, @"^(.*?)\?.*$", "$1");

        string localName = Path.Combine(albumDirectory, Path.GetFileName(rawName));

        TrackDownloadResult downloadResult = await DownloadTrack(track, localName, token);

        if (downloadResult == TrackDownloadResult.Success)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        downloadStatuses[track] = downloadResult;

        semaphor.Release();
    }
    private async Task<TrackDownloadResult> DownloadTrack(TrackDownloadModel track, string localName, CancellationToken? token = null)
    {
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

        var startDownloadTime = DateTime.UtcNow;

        try
        {
            var bytes = await client.GetByteArrayAsync(track.DownloadUri, token ?? CancellationToken.None);
            await File.WriteAllBytesAsync(localName, bytes, token ?? CancellationToken.None);

            await apiClient.LogDownloadHistory(new(track.TrackId, startDownloadTime, DateTime.UtcNow, bytes.Length));

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
        catch
        {
            return TrackDownloadResult.Error;
        }
    }
    public void Dispose()
    {
        client?.Dispose();
        semaphor?.Dispose();
    }


}


