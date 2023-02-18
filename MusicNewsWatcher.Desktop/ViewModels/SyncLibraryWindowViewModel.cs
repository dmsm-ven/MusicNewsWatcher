using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class SyncLibraryWindowViewModel : ViewModelBase
{
    private readonly ISyncLibraryTracker tracker;

    bool inProgress = true;
    public bool InProgress
    {
        get => inProgress;
        set => Set(ref inProgress, value);
    }

    bool isAppendClicked = false;
    public bool IsAppendClicked
    {
        get => isAppendClicked;
        set
        {
            if (Set(ref isAppendClicked, value))
            {
                RaisePropertyChanged(nameof(AddIconState));
            }
        }
    }

    string newHostName;
    public string NewHostName { get => newHostName; set => Set(ref newHostName, value); }

    string newHostRootFolderPath;
    public string NewHostRootFolderPath { get => newHostRootFolderPath; set => Set(ref newHostRootFolderPath, value); }

    public PackIconFontAwesomeKind AddIconState
    {
        get => isAppendClicked ? PackIconFontAwesomeKind.CheckSolid : PackIconFontAwesomeKind.PlusSolid;
    }

    public ICommand LoadedCommand { get; }
    public ICommand AddNewSyncHostCommand { get; }
    public ICommand RemoveSelectedSyncHostCommand { get; }

    public ObservableCollection<SyncHostViewModel> Hosts { get; }

    public SyncLibraryWindowViewModel()
    {
        LoadedCommand = new LambdaCommand(async (e) => await Loaded());
        AddNewSyncHostCommand = new LambdaCommand(async (e) => await AddNewSyncHost(), CanAddNewHost);
        RemoveSelectedSyncHostCommand = new LambdaCommand(_ => { });

        Hosts = new ObservableCollection<SyncHostViewModel>();
    }

    private bool CanAddNewHost(object arg)
    {
        return !IsAppendClicked || 
            (IsAppendClicked && 
            !string.IsNullOrWhiteSpace(NewHostName) && 
            !string.IsNullOrWhiteSpace(NewHostRootFolderPath) &&
            Directory.Exists(NewHostRootFolderPath));
    }

    public SyncLibraryWindowViewModel(ISyncLibraryTracker tracker) : this()
    {
        this.tracker = tracker;
    }

    private async Task AddNewSyncHost()
    {
        if (IsAppendClicked)
        {
            await tracker.CreateHost(new SyncHostEntity()
            {
                Id = Guid.NewGuid(),
                Name = NewHostName,
                RootFolderPath = NewHostRootFolderPath
            })
            IsAppendClicked = false;
        }
        else
        {
            IsAppendClicked = true;
        }

        
    }

    private async Task Loaded()
    {
        try
        {
            InProgress = true;

            var list = await tracker.GetAllSyncHosts();
            foreach (var item in list)
            {
                Hosts.Add(new SyncHostViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    RootFolderPath = item.RootFolderPath,
                });
            }
        }
        finally
        {
            InProgress = false;
        }




    }
}

public class SyncHostViewModel : ViewModelBase
{
    public Guid Id { get; init; }

    string name = string.Empty;
    public string Name { get => name; set => Set(ref name, value); }

    string rootFolderPath = string.Empty;
    public string RootFolderPath { get => rootFolderPath; set => Set(ref rootFolderPath, value); }
}


