using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.ViewModels;

public partial class SyncHostViewModel(ISyncLibraryTracker tracker) : ObservableObject
{
    public Guid Id { get; init; }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(UploadTracksCommand))]
    private string rootFolderPath = string.Empty;

    [ObservableProperty]
    private DateTimeOffset? lastUpdate = null;

    [ObservableProperty]
    private PackIconFontAwesomeKind icon;

    [ObservableProperty]
    private ObservableCollection<string> tracks = new();

    [RelayCommand(CanExecute = nameof(CanCheckDiffWithCommand))]
    private async Task CheckDiffWith(object e)
    {
        throw new NotImplementedException();
        //if (!(e is SyncHostViewModel otherHost))
        //{
        //    return;
        //}

        //var diffLines = await tracker.GetTracksDiff(this.Id, otherHost.Id);

        //if (diffLines.Length > 0)
        //{
        //    string msg = new StringBuilder()
        //        .AppendLine($"Найдены {diffLines.Length} новых файла(ов), в сравнении с текущей библиотекой")
        //        .AppendLine($"Данные записаны в файл '{MISSING_TRACKS_FILE}'")
        //        .Append("Открыть файл ?")
        //        .ToString();
        //    var result = MessageBox.Show(msg, "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);

        //    File.WriteAllText(MISSING_TRACKS_FILE, string.Join("\r\n", diffLines));

        //    if (result == MessageBoxResult.Yes)
        //    {
        //        Process.Start(new ProcessStartInfo { FileName = "notepad++", Arguments = MISSING_TRACKS_FILE, UseShellExecute = true });
        //    }
        //}
        //else
        //{
        //    MessageBox.Show("Библиотеки идентичны", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        //}
    }

    private bool CanCheckDiffWithCommand(object e)
    {
        return (e as SyncHostViewModel) != this && e != null;
    }

    [RelayCommand]
    private async Task LoadItems()
    {
        throw new NotImplementedException();
        var files = Directory
            .GetFiles(RootFolderPath, "*.*", SearchOption.AllDirectories)
            .Select(fi => new FileInfo(fi))
            .Select(file => file.FullName.Replace(RootFolderPath, string.Empty).Trim('\\'))
            .ToArray();

        await tracker.AddTrackedItemsForHost(Id, files);

        await LoadTracks();
    }

    [RelayCommand]
    private async Task LoadTracks()
    {
        throw new NotImplementedException();
        Tracks.Clear();

        var tracks = await tracker.GetAllTracksForHost(Id);

        foreach (var t in tracks)
        {
            Tracks.Add(t.Path);
        }
    }

    [RelayCommand(CanExecute = nameof(CanUploadTracksCommand))]
    private async Task UploadTracks()
    {
        throw new NotImplementedException();
    }

    private bool CanUploadTracksCommand()
    {
        return Directory.Exists(RootFolderPath);
    }
}


