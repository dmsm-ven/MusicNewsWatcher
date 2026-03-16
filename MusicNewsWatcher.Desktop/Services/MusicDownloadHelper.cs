using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.ViewModels.Items;

namespace MusicNewsWatcher.Desktop.Services;

public class MusicDownloadHelper
{
    private readonly MultithreadHttpDownloadManager musicDownloadManager;
    private readonly IToastsNotifier toasts;
    private readonly ILogger<MusicDownloadHelper> logger;
    private readonly string musicDownloadFolder;

    public MusicDownloadHelper(MultithreadHttpDownloadManager musicDownloadManager,
        IToastsNotifier toasts,
        IOptions<MusicDownloadFolderOptions> options,
        ILogger<MusicDownloadHelper> logger)
    {
        this.musicDownloadManager = musicDownloadManager;
        this.toasts = toasts;
        this.logger = logger;
        this.musicDownloadFolder = options.Value.MusicDownloadFolder ?? throw new Exception("MusicDownloadHelper options not provided");
        this.musicDownloadManager.ThreadLimit = 1;
    }

    public async Task DownloadAlbum(AlbumViewModel album, CancellationToken token)
    {
        album.InProgress = true;

        Action<TrackDownloadModel, TrackDownloadResult> statusChangedAction = (track, status) =>
        {
            var trackVm = album.Tracks.Single(t => t.Id == track.TrackId);
            trackVm.DownloadResult = status;
        };

        try
        {
            this.musicDownloadManager.TrackDownloadResultChanged += statusChangedAction;

            await DownloadAlbumTracks(album, token);

            toasts.ShowSuccess($"Альбом загружен: {album.Title}");
        }
        catch (OperationCanceledException)
        {
            toasts.ShowError($"Загрузка альбома отменена");
        }
        catch (Exception ex)
        {
            toasts.ShowError($"Ошибка загрузки альбома '{album.Title}'\r\n{ex.Message}");
        }
        finally
        {
            album.InProgress = false;
            this.musicDownloadManager.TrackDownloadResultChanged -= statusChangedAction;
        }
    }

    public async Task DownloadCheckedAlbums(IEnumerable<AlbumViewModel> albums)
    {
        using CancellationTokenSource cts = new();

        foreach (var album in albums)
        {
            await DownloadAlbum(album, cts.Token);
        }
    }

    private async Task DownloadAlbumTracks(AlbumViewModel album, CancellationToken token)
    {
        var albumModel = new AlbumDownloadModel()
        {
            AlbumDisplayName = album.Title,
            ArtistDisplayName = album.ParentArtist.Name,
            Tracks = album.Tracks.Select(t => new TrackDownloadModel()
            {
                DownloadUri = t.DownloadUri,
                TrackId = t.Id
            }).ToList()
        };

        await musicDownloadManager.DownloadFullAlbum(albumModel, musicDownloadFolder, token);
    }
}
