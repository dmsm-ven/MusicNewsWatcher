namespace MusicNewsWatcher.Core;
public interface IMusicDownloadManager
{
    int ThreadLimit { get; set; }
    Task<string> DownloadFullAlbum(string albumUri, string downloadDirectory, CancellationToken? token = null);
}