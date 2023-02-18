using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class SyncHostViewModel : ViewModelBase
{
    public Guid Id { get; init; }

    string name = string.Empty;
    public string Name { get => name; set => Set(ref name, value); }

    string rootFolderPath = string.Empty;
    public string RootFolderPath { get => rootFolderPath; set => Set(ref rootFolderPath, value); }

    DateTimeOffset? lastUpdate = null;
    public DateTimeOffset? LastUpdate { get => lastUpdate; set => Set(ref lastUpdate, value); }

    PackIconFontAwesomeKind icon;
    private readonly ISyncLibraryTracker tracker;

    public PackIconFontAwesomeKind Icon
    {
        get => icon;
        set => Set(ref icon, value);
    }

    public ICommand LoadItemsCommand { get; }
    public ICommand UploadTracksCommand { get; }

    public ObservableCollection<string> Tracks { get; }

    public SyncHostViewModel()
    {
        Tracks = new ObservableCollection<string>();
        LoadItemsCommand = new LambdaCommand(async (e) => await LoadTracks());
        UploadTracksCommand = new LambdaCommand(async (e) => await UploadTracks(), e => Directory.Exists(RootFolderPath));
    }

    public SyncHostViewModel(ISyncLibraryTracker tracker) : this()
    {

        this.tracker = tracker;
    }

    private async Task UploadTracks()
    {
        var files = Directory
            .GetFiles(RootFolderPath, "*.*", SearchOption.AllDirectories)
            .Select(fi => new FileInfo(fi))
            .Select(file => file.FullName.Replace(RootFolderPath, string.Empty).Trim('\\'))
            .ToArray();

        await tracker.AddTrackedItemsForHost(Id, files);
    }

    private async Task LoadTracks()
    {
        if (Tracks.Any()) { return; }

        var tracks = await tracker.GetAllTracksForHost(Id);

        foreach (var t in tracks)
        {
            Tracks.Add(t.Path);
        }
    }
}


