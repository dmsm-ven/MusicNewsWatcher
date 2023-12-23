using Microsoft.Extensions.Logging;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Desktop.Models.ViewModels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.Services;

public class MusicDownloadHelper
{
    private readonly IMusicDownloadManager musicDownloadManager;
    private readonly IToastsNotifier toasts;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly ILogger<MusicDownloadHelper> logger;

    public MusicDownloadHelper(IMusicDownloadManager musicDownloadManager,
        IToastsNotifier toasts,
        IDbContextFactory<MusicWatcherDbContext> dbFactory,
        ILogger<MusicDownloadHelper> logger)
    {
        this.musicDownloadManager = musicDownloadManager;
        this.toasts = toasts;
        this.dbFactory = dbFactory;
        this.logger = logger;
    }

    public async Task DownloadAlbum(AlbumViewModel album, bool openFolder, CancellationToken token)
    {
        album.InProgress = true;

        using (var db = dbFactory.CreateDbContext())
        {
            int parallelDownloads = int.Parse(db.Settings?.Find("DownloadThreadsNumber")?.Value ?? "1");

            musicDownloadManager.ThreadLimit = parallelDownloads;
        }

        try
        {
            logger.LogInformation("Начало загрузки альбома {albumName}", album.DisplayName);
            await DownloadAlbumTracks(album, openFolder, token);
            logger.LogInformation("Конец загрузки альбома {albumName}", album.DisplayName);

            toasts.ShowSuccess($"Альбом загружен: {album.DisplayName}");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Загрузка альбома отменена {albumName}", album.DisplayName);
            toasts.ShowError($"Загрузка альбома отменена");
        }
        catch (Exception ex)
        {
            logger.LogWarning("Ошибка загрузки альбома '{albumName} - {msg}'", album.DisplayName, ex.Message);
            toasts.ShowError($"Ошибка загрузки альбома '{album.DisplayName}'\r\n{ex.Message}");
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
            AlbumDisplayName = album.DisplayName,
            ArtistDisplayName = album.ParentArtist.DisplayName,
            Tracks = album.Tracks.Select(t => new TrackModel()
            {
                DownloadUri = t.DownloadUri
            }).ToList()
        };

        string albumDir = await musicDownloadManager.DownloadFullAlbum(albumModel, FileBrowserHelper.DownloadDirectory, token);

        await Task.Delay(TimeSpan.FromSeconds(1));

        if (openFolderAfterDownload)
        {
            FileBrowserHelper.OpenFolderInFileBrowser(albumDir);
        }
    }
}
