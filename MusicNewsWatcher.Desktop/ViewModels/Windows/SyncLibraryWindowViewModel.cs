using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class SyncLibraryWindowViewModel(ISyncLibraryTracker tracker) : ObservableObject
{
    [ObservableProperty]
    private bool inProgress = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AddIconState))]
    private bool isAppendClicked = false;

    public PackIconFontAwesomeKind AddIconState
    {
        get => IsAppendClicked ? PackIconFontAwesomeKind.CheckSolid : PackIconFontAwesomeKind.PlusSolid;
    }

    [ObservableProperty]
    private ObservableCollection<SyncHostViewModel> hosts = new();

    [ObservableProperty]
    private SyncHostViewModel selectedHost;

    [ObservableProperty]
    private SyncHostViewModel newHost = new(tracker);

    private bool CanAddNewHost()
    {
        throw new NotImplementedException();
        //return !IsAppendClicked ||
        //    (IsAppendClicked &&
        //    !string.IsNullOrWhiteSpace(NewHost.Name) &&
        //    !string.IsNullOrWhiteSpace(NewHost.RootFolderPath) &&
        //    Directory.Exists(NewHost.RootFolderPath));
    }

    [RelayCommand(CanExecute = nameof(CanAddNewHost))]
    private async Task AddNewSyncHost()
    {
        throw new NotImplementedException();
        /*
         * if (IsAppendClicked)
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
         */
    }

    [RelayCommand]
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


