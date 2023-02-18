using MahApps.Metro.IconPacks;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class SyncHostViewModel : ViewModelBase
{
    public Guid Id { get; init; }

    string name = string.Empty;
    public string Name { get => name; set => Set(ref name, value); }

    string rootFolderPath = string.Empty;
    public string RootFolderPath { get => rootFolderPath; set => Set(ref rootFolderPath, value); }

    PackIconFontAwesomeKind icon;
    public PackIconFontAwesomeKind Icon
    {
        get => icon;
        set => Set(ref icon, value);
    }
}


