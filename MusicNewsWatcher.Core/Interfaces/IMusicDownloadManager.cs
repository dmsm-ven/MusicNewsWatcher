using MusicNewsWatcher.Core.Models;

namespace MusicNewsWatcher.Core.Interfaces;

public interface IMusicDownloadManager
{
    int ThreadLimit { get; set; }
    Task DownloadFullAlbum(AlbumDownloadModel album, string downloadDirectory, CancellationToken? token = null);
}