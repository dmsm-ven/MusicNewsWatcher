﻿using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.Extensions;
using MusicNewsWatcher.Core.Models;
using System.Net;

namespace MusicNewWatcher.BL;

//TODO убрать прямое обращение через ViewModel
public class SimpleHttpMusicDownloadManager : IMusicDownloadManager
{
    private readonly ILogger<SimpleHttpMusicDownloadManager> logger;
    private readonly HttpClient client;
    private SemaphoreSlim? semaphor = new(1);

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

    public SimpleHttpMusicDownloadManager(ILogger<SimpleHttpMusicDownloadManager> logger)
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

        this.logger = logger;
    }

    /// <summary>
    /// Загружает альбом по указанному URI и возращает путь до созданной папки в которую были загружены все треки альбома 
    /// </summary>
    /// <param name="albumUri"></param>
    /// <param name="downloadDirectory"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<string> DownloadFullAlbum(AlbumModel album, string downloadDirectory, CancellationToken? token = null)
    {
        logger.LogInformation("Начало загрузки альбома {albumName} в {} потока(ов)", album.AlbumDisplayName, ThreadLimit);

        string albumDirectory = GetAlbumLocalPath(album, downloadDirectory);

        album.Tracks.ToList().ForEach(i => i.SetDownloadResult(TrackDownloadResult.None));

        var tasks = album.Tracks.Select(track => CreateDownloadTrackTask(track, albumDirectory, token));

        await Task.WhenAll(tasks);

        return albumDirectory;
    }

    private string GetAlbumLocalPath(AlbumModel album, string downloadDirectory)
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

    private async Task CreateDownloadTrackTask(TrackModel track, string albumDirectory, CancellationToken? token = null)
    {
        if (string.IsNullOrWhiteSpace(track.DownloadUri) || !Uri.IsWellFormedUriString(track.DownloadUri, UriKind.Absolute))
        {
            return;
        }

        await semaphor!.WaitAsync(token ?? CancellationToken.None);

        string localName = Path.Combine(albumDirectory, Path.GetFileName(track.DownloadUri));

        TrackDownloadResult downloadResult = await DownloadTrack(track, localName, token);

        if (downloadResult == TrackDownloadResult.Success)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        track.SetDownloadResult(downloadResult);

        semaphor.Release();
    }

    private async Task<TrackDownloadResult> DownloadTrack(TrackModel track, string localName, CancellationToken? token = null)
    {
        if (File.Exists(localName))
        {
            //Файл не был до конца скачан, - тут хорошо бы сделать проверку на целостность файла
            //Т.к. файл может быть битым не только когда он 0 Байт, а когда скачался не полностью
            //Возможность стоит в будущем сделать проверку CRC целосности файла
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
            var bytes = await client.GetByteArrayAsync(track.DownloadUri, token ?? CancellationToken.None);
            await File.WriteAllBytesAsync(localName, bytes, token ?? CancellationToken.None);
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


