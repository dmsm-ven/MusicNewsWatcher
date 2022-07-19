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

    public async Task<string> DownloadFullAlbum(AlbumViewModel album,
        IProgress<ValueTuple<TrackViewModel, bool>> indicator, 
        CancellationToken token)
    {
        // Скорее всего нельзя ставить больше 2х,
        // т.к. многие сайты не позволяют закачивать больше чем в 2 потока
        const int PARALLEL_DOWNLOADS = 2;
        string albumDirectory = Path.Combine(DOWNLOAD_DIRECTORY, 
            album.ParentArtistName.RemoveInvalidCharacters(), 
            album.DisplayName.RemoveInvalidCharacters());  
        
        IList<TrackViewModel> source = album.Tracks;
        int currentPage = 0;

        for(int i = 0; i < source.Count; i+= PARALLEL_DOWNLOADS)
        {
            var chunk = source.Skip(currentPage * PARALLEL_DOWNLOADS).Take(PARALLEL_DOWNLOADS)
                .Select(t => CreateDownloadTrackTask(t, albumDirectory, indicator, token))
                .ToArray();

            await Task.WhenAll(chunk);

            if (token.IsCancellationRequested)
            {
                break;
            }

            currentPage++;
        }

        return albumDirectory;
    }

    private async Task CreateDownloadTrackTask(TrackViewModel track,string albumDirectory,
        IProgress<ValueTuple<TrackViewModel, bool>> indicator, CancellationToken token)
        {
            indicator.Report(ValueTuple.Create(track, false));

            bool isLoaded = await DownloadTrack(albumDirectory, track, token);

            if (isLoaded)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            indicator.Report(ValueTuple.Create(track, true));
        }

    private async Task<bool> DownloadTrack(string albumDirectory, TrackViewModel track, CancellationToken token)
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
            var bytes = await client.GetByteArrayAsync(track.DownloadUri, token);
            await File.WriteAllBytesAsync(localName, bytes);
            return true;
        }
        catch (TaskCanceledException)
        {
            if (File.Exists(localName))
            {
                File.Delete(localName);
            }
        }
        catch(Exception ex)
        {
            return false;
        }

        return false;
    }
}
