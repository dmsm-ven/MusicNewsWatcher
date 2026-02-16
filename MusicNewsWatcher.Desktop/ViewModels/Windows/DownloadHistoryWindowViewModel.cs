using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Humanizer;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using System.Collections.ObjectModel;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class DownloadHistoryWindowViewModel(MusicNewsWatcherApiClient apiClient) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<TrackDownloadHistoryItem> items = new();

    [RelayCommand]
    private async Task Loaded()
    {
        var history = await apiClient.GetDownloadHistory(limit: 1000);

        var mappedItems = history.Select(i => new TrackDownloadHistoryItem()
        {
            TrackName = i.TrackName,
            DownloadStarted = i.Started,
            DownloadTimeHumanized = (i.Finished - i.Started).Humanize(),
            DownloadStartedElapsed = (DateTime.Now - i.Started).Humanize(),
            FileSizeHumanized = i.FileSizeInBytes.Bytes().Humanize(),
            ArtistName = i.ArtistName,
            AlbumName = i.AlbumName
        });

        Items.AddRange(mappedItems);
    }
}

public class TrackDownloadHistoryItem
{
    public string TrackName { get; init; } = string.Empty;
    public DateTime DownloadStarted { get; init; }
    public string ArtistName { get; init; } = string.Empty;
    public string AlbumName { get; init; } = string.Empty;
    public string DownloadStartedElapsed { get; init; } = string.Empty;
    public string DownloadUri { get; init; } = string.Empty;
    public string DownloadTimeHumanized { get; init; } = string.Empty;
    public string FileSizeHumanized { get; init; } = string.Empty;
}
