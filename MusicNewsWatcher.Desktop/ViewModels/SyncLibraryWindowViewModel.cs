using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;
using MusicNewsWatcher.Desktop.ViewModels.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class SyncLibraryWindowViewModel : ViewModelBase
{
    private readonly ISyncLibraryTracker tracker;
    private bool inProgress = true;
    public bool InProgress
    {
        get => inProgress;
        set => Set(ref inProgress, value);
    }

    private bool isAppendClicked = false;
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

    public PackIconFontAwesomeKind AddIconState
    {
        get => isAppendClicked ? PackIconFontAwesomeKind.CheckSolid : PackIconFontAwesomeKind.PlusSolid;
    }

    public ICommand LoadedCommand { get; }
    public ICommand AddNewSyncHostCommand { get; }
    public ICommand RemoveSelectedSyncHostCommand { get; }

    public ObservableCollection<SyncHostViewModel> Hosts { get; }

    private SyncHostViewModel selectedHost;
    public SyncHostViewModel SelectedHost
    {
        get => selectedHost;
        set
        {
            if (Set(ref selectedHost, value))
            {
                selectedHost.LoadItemsCommand.Execute(null);
            }
        }
    }

    private SyncHostViewModel newHost;
    public SyncHostViewModel NewHost
    {
        get => newHost;
        set => Set(ref newHost, value);
    }

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
            !string.IsNullOrWhiteSpace(NewHost.Name) &&
            !string.IsNullOrWhiteSpace(NewHost.RootFolderPath) &&
            Directory.Exists(NewHost.RootFolderPath));
    }

    public SyncLibraryWindowViewModel(ISyncLibraryTracker tracker) : this()
    {
        this.tracker = tracker;
        this.newHost = new SyncHostViewModel(tracker);
    }

    private async Task AddNewSyncHost()
    {
        if (IsAppendClicked)
        {
            try
            {
                InProgress = true;
                var newHostEntity = new SyncHostEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = NewHost.Name,
                    RootFolderPath = NewHost.RootFolderPath,
                    Icon = nameof(PackIconFontAwesomeKind.QuestionSolid)
                };
                await tracker.CreateHost(newHostEntity);
            }
            finally
            {
                NewHost = new SyncHostViewModel(tracker);
                InProgress = false;
                IsAppendClicked = false;
            }
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
                if (!Enum.TryParse<PackIconFontAwesomeKind>(item.Icon, out var iconKind))
                {
                    iconKind = PackIconFontAwesomeKind.QuestionSolid;
                }
                Hosts.Add(new SyncHostViewModel(tracker)
                {
                    Id = item.Id,
                    Name = item.Name,
                    RootFolderPath = item.RootFolderPath,
                    Icon = iconKind,
                    LastUpdate = item.LastUpdate
                });
            }
        }
        finally
        {
            InProgress = false;
        }




    }
}


