using MusicNewsWatcher.Core.Models;

namespace MusicNewsWatcher.Core.Interfaces;

public interface IMusicDownloadManager
{
    int ThreadLimit { get; set; }
    Task<string> DownloadFullAlbum(AlbumModel album, string downloadDirectory, CancellationToken? token = null);
}