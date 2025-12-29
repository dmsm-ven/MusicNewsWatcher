using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.Core.Interfaces;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using System.Diagnostics;

namespace MusicNewsWatcher.Desktop.Services;

public class MusicDownloadHelper
{
    private readonly IMusicDownloadManager musicDownloadManager;
    private readonly IToastsNotifier toasts;
    private readonly ILogger<MusicDownloadHelper> logger;
    private readonly string musicDownloadFolder;

    public MusicDownloadHelper(IMusicDownloadManager musicDownloadManager,
        IToastsNotifier toasts,
        ILogger<MusicDownloadHelper> logger,
        IOptions<MusicDownloadFolderOptions> options)
    {
        this.musicDownloadManager = musicDownloadManager;
        this.toasts = toasts;
        this.logger = logger;
        this.musicDownloadFolder = options.Value.MusicDownloadFolder;
    }

    public async Task DownloadAlbum(AlbumViewModel album, bool openFolder, CancellationToken token)
    {
        album.InProgress = true;

        int parallelDownloads = int.Parse(dbContext.Settings?.Find("DownloadThreadsNumber")?.Value ?? "1");

        musicDownloadManager.ThreadLimit = parallelDownloads;


        try
        {
            logger.LogInformation("Начало загрузки альбома {albumName}", album.Title);
            await DownloadAlbumTracks(album, openFolder, token);
            logger.LogInformation("Конец загрузки альбома {albumName}", album.Title);

            toasts.ShowSuccess($"Альбом загружен: {album.Title}");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Загрузка альбома отменена {albumName}", album.Title);
            toasts.ShowError($"Загрузка альбома отменена");
        }
        catch (Exception ex)
        {
            logger.LogWarning("Ошибка загрузки альбома '{albumName} - {msg}'", album.Title, ex.Message);
            toasts.ShowError($"Ошибка загрузки альбома '{album.Title}'\r\n{ex.Message}");
        }
        finally
        {
            album.InProgress = false;
        }
    }

    public async Task DownloadCheckedAlbums(IEnumerable<AlbumViewModel> albums)
    {
        using CancellationTokenSource cts = new();

        foreach (var album in albums)
        {
            await DownloadAlbum(album, false, cts.Token);
        }
    }

    private async Task DownloadAlbumTracks(AlbumViewModel album, bool openFolderAfterDownload, CancellationToken token)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var albumModel = new AlbumModel()
        {
            AlbumDisplayName = album.Title,
            ArtistDisplayName = album.ParentArtist.Name,
            Tracks = album.Tracks.Select(t => new TrackModel()
            {
                DownloadUri = t.DownloadUri
            }).ToList()
        };

        string albumDir = await musicDownloadManager.DownloadFullAlbum(albumModel, musicDownloadFolder, token);

        await Task.Delay(TimeSpan.FromSeconds(1));

        if (openFolderAfterDownload)
        {
            FileBrowserHelper.OpenFolderInFileBrowser(albumDir);
        }
    }
}
