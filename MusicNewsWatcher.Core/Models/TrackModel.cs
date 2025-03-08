namespace MusicNewsWatcher.Core.Models;

public class TrackModel
{
    public event Action? OnDownloadResultChanged;

    public string? DownloadUri { get; init; }

    private readonly TrackDownloadResult downloadResult;
    public TrackDownloadResult DownloadResult
    {
        get => downloadResult;
        private set
        {
            if (downloadResult != value)
            {
                value = downloadResult;
                OnDownloadResultChanged?.Invoke();
            }
        }
    }

    public void SetDownloadResult(TrackDownloadResult newState)
    {
        DownloadResult = newState;
    }
}