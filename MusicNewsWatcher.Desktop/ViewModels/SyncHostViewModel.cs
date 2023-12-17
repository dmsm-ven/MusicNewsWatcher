using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;
using MusicNewsWatcher.Desktop.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class SyncHostViewModel : ViewModelBase
{
    private string MISSING_TRACKS_FILE
    {
        get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "missing.txt");
    }

    public Guid Id { get; init; }

    private string name = string.Empty;
    public string Name { get => name; set => Set(ref name, value); }

    private string rootFolderPath = string.Empty;
    public string RootFolderPath { get => rootFolderPath; set => Set(ref rootFolderPath, value); }

    private DateTimeOffset? lastUpdate = null;
    public DateTimeOffset? LastUpdate { get => lastUpdate; set => Set(ref lastUpdate, value); }

    private PackIconFontAwesomeKind icon;
    private readonly ISyncLibraryTracker tracker;

    public PackIconFontAwesomeKind Icon
    {
        get => icon;
        set => Set(ref icon, value);
    }

    public ICommand LoadItemsCommand { get; }
    public ICommand UploadTracksCommand { get; }
    public ICommand CheckDiffWithCommand { get; }

    public ObservableCollection<string> Tracks { get; }

    public SyncHostViewModel()
    {
        Tracks = new ObservableCollection<string>();
        LoadItemsCommand = new LambdaCommand(async (e) => await LoadTracks());
        CheckDiffWithCommand = new LambdaCommand(async (e) => await CheckDiffWith(e), e => (e as SyncHostViewModel) != this && e != null);
        UploadTracksCommand = new LambdaCommand(async (e) => await UploadTracks(), e => Directory.Exists(RootFolderPath));
    }

    private async Task CheckDiffWith(object e)
    {
        if (!(e is SyncHostViewModel otherHost))
        {
            return;
        }

        var diffLines = await tracker.GetTracksDiff(this.Id, otherHost.Id);

        if (diffLines.Length > 0)
        {
            string msg = new StringBuilder()
                .AppendLine($"Найдены {diffLines.Length} новых файла(ов), в сравнении с текущей библиотекой")
                .AppendLine($"Данные записаны в файл '{MISSING_TRACKS_FILE}'")
                .Append("Открыть файл ?")
                .ToString();
            var result = MessageBox.Show(msg, "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);

            File.WriteAllText(MISSING_TRACKS_FILE, string.Join("\r\n", diffLines));

            if (result == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo { FileName = "notepad++", Arguments = MISSING_TRACKS_FILE, UseShellExecute = true });
            }
        }
        else
        {
            MessageBox.Show("Библиотеки идентичны", "Информация", MessageBoxButton.OK, MessageBoxImage.Information); ;
        }
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

        await LoadTracks();
    }

    private async Task LoadTracks()
    {
        Tracks.Clear();

        var tracks = await tracker.GetAllTracksForHost(Id);

        foreach (var t in tracks)
        {
            Tracks.Add(t.Path);
        }
    }
}


