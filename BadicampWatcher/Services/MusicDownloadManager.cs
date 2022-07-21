using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewsWatcher.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Services;

//TODO убрать прямое обращение через ViewModel
public class MusicDownloadManager
{
    private readonly string DOWNLOAD_DIRECTORY;
    private readonly HttpClient client;

    public MusicDownloadManager(string downloadDirectory)
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
        this.DOWNLOAD_DIRECTORY = downloadDirectory;
    }

    public async Task<string> DownloadFullAlbum(AlbumViewModel album, int parallelDownloads, CancellationToken token)
    {
        if(parallelDownloads <= 0 || parallelDownloads > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(parallelDownloads), "Parallel downloads must be in range 1..32");
        }

        string albumDirectory = GetAlbumLocalPath(album); 
        
        IList<TrackViewModel> source = album.Tracks;
        int currentPage = 0;

        for(int i = 0; i < source.Count; i+= parallelDownloads)
        {
            var chunk = source.Skip(currentPage * parallelDownloads).Take(parallelDownloads)
                .Select(t => CreateDownloadTrackTask(t, albumDirectory, token))
                .ToArray();

            await Task.WhenAll(chunk);

            if (token.IsCancellationRequested) break;

            currentPage++;
        }

        return albumDirectory;
    }

    private async Task CreateDownloadTrackTask(TrackViewModel track, string albumDirectory, CancellationToken token)
    {
        track.IsDownloading = true;

        string localName = Path.Combine(albumDirectory, Path.GetFileName(track.DownloadUri));

        TrackDownloadResult downloadResult = await DownloadTrack(track, localName, token);

        if (downloadResult == TrackDownloadResult.Success)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        track.IsDownloading = false;
        track.DownloadResult = downloadResult;
    }
    
    private async Task<TrackDownloadResult> DownloadTrack(TrackViewModel track, string localName, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(track.DownloadUri))
        {
            return TrackDownloadResult.Error;
        }

        if(File.Exists(localName))
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
        catch(Exception ex)
        {
            return TrackDownloadResult.Error;
        }

        return TrackDownloadResult.Error;
    }

    private string GetAlbumLocalPath(AlbumViewModel album)
    {
        var directoryPath = Path.Combine(DOWNLOAD_DIRECTORY,
            album.ParentArtistName.RemoveInvalidCharacters(),
            album.DisplayName.RemoveInvalidCharacters());

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return directoryPath;
    }
}

public enum TrackDownloadResult
{
    None,
    Error,
    Success,
    Skipped,
    Cancelled
}
