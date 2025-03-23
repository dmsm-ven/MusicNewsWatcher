using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.Models.ViewModels;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.ViewModels;
public partial class MainWindowViewModel(MusicUpdateManager updateManager,
        IEnumerable<MusicProviderBase> musicProviderTemplates,
        MusicWatcherDbContext dbContext,
        IToastsNotifier toasts,
        IDialogWindowService dialogWindowService,
        IOptions<MusicDownloadFolderOptions> options,
        ViewModelFactory<MusicProviderViewModel> providerVmFactory) : ObservableObject
{

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private MusicProviderViewModel? selectedMusicProvider;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddArtistCommand))]
    private ObservableCollection<MusicProviderViewModel> musicProviders = new();

    public bool HasSelectedProvider => SelectedMusicProvider != null;

    [RelayCommand(CanExecute = nameof(HasSelectedProvider))]
    private void AddArtist()
    {
        dialogWindowService.ShowNewArtistWindow(SelectedMusicProvider);
    }

    [RelayCommand]
    private void OpenDownloadsFolder()
    {
        FileBrowserHelper.OpenFolderInFileBrowser(options.Value.MusicDownloadFolder);
    }

    [RelayCommand]
    private void Settings()
    {
        dialogWindowService.ShowSettingsWindow();

    }

    [RelayCommand]
    private void SyncLibrary()
    {
        dialogWindowService.ShowSyncLibraryWindow();
    }

    [RelayCommand]
    private async Task Loaded()
    {
        await CheckUpdatesAll();
    }

    [RelayCommand]
    private async Task CheckUpdatesAll()
    {
        SelectedMusicProvider = null;
        MusicProviders.Clear();

        var templatesInfo = musicProviderTemplates.ToDictionary(i => i.Id, i => i);

        IsLoading = true;

        var source = await dbContext.MusicProviders
            .Include(p => p.Artists)
            .ToListAsync();

        foreach (var pinfo in source)
        {
            var providerVm = providerVmFactory.Create();

            providerVm.Initialize(templatesInfo[pinfo.MusicProviderId], pinfo.Image, pinfo.Uri, pinfo.Artists.Count);

            providerVm.PropertyChanged += ProviderVm_PropertyChanged;
            MusicProviders.Add(providerVm);
        }

        IsLoading = false;
        SelectedMusicProvider = MusicProviders.LastOrDefault();
    }

    private void ProviderVm_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var provider = (MusicProviderViewModel)sender;

        if (e.PropertyName == nameof(MusicProviderViewModel.IsActiveProvider) && provider!.IsActiveProvider)
        {
            this.musicProviders
                .Where(p => p != provider)
                .ToList()
                .ForEach(p => p.IsActiveProvider = false);
        }
    }
}
