using MusicNewsWatcher.Core.Models;

namespace MusicNewsWatcher.Core;
public interface IMusicDownloadManager
{
    int ThreadLimit { get; set; }
    Task<string> DownloadFullAlbum(AlbumModel album, string downloadDirectory, CancellationToken? token = null);
}