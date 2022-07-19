using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewsWatcher.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Services;

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

    public async Task<string> DownloadFullAlbum(AlbumViewModel album, Action<TrackViewModel> progressChanged, CancellationToken token)
    {
        string authorClear = album.ParentArtistName;
        string albumClear = album.DisplayName;
        string albumDirectory = Path.Combine(DOWNLOAD_DIRECTORY, authorClear, albumClear);


        foreach (var track in album.Tracks)
        {
            progressChanged?.Invoke(track);
            bool isLoaded = await DownloadTrack(albumDirectory, track);

            if (isLoaded)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            progressChanged?.Invoke(track);

            if (token.IsCancellationRequested)
            {
                break;
            }
        }

        return albumDirectory;
    }

    private async Task<bool> DownloadTrack(string albumDirectory, TrackViewModel track)
    {
        if (!Directory.Exists(albumDirectory))
        {
            Directory.CreateDirectory(albumDirectory);
        }

        string localName = Path.Combine(albumDirectory, Path.GetFileName(track.DownloadUri));

        if(File.Exists(localName))
        {
            if (new FileInfo(localName).Length == 0)
            {
                File.Delete(localName);
            }
            else
            {
                // пропускаем, файл уже скачан
                return false;
            }
        }

        try
        {
            var bytes = await client.GetByteArrayAsync(track.DownloadUri);
            await File.WriteAllBytesAsync(localName, bytes);
            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}
