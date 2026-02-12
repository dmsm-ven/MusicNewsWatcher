using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using System.Diagnostics;

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



        try
        {
            logger.LogInformation("Начало загрузки альбома {albumName}", album.Title);
            await DownloadAlbumTracks(album, token);
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
            await DownloadAlbum(album, cts.Token);
        }
    }

    private async Task DownloadAlbumTracks(AlbumViewModel album, CancellationToken token)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var albumModel = new AlbumDownloadModel()
        {
            AlbumDisplayName = album.Title,
            ArtistDisplayName = album.ParentArtist.Name,
            Tracks = album.Tracks.Select(t => new TrackDownloadModel()
            {
                DownloadUri = t.DownloadUri
            }).ToList()
        };

        await musicDownloadManager.DownloadFullAlbum(albumModel, musicDownloadFolder, token);

    }
}
