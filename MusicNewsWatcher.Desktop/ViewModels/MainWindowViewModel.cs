using MusicNewsWatcher.Desktop.Models.ViewModels;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IEnumerable<MusicProviderBase> musicProviders;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;
    private readonly IToastsNotifier toasts;
    private readonly IDialogWindowService dialogWindowService;

    public string Title
    {
        get
        {
            string title = "Music News Watcher";

            return title;
        }
    }

    private bool isLoading = true;
    public bool IsLoading
    {
        get => isLoading;
        set => Set(ref isLoading, value);
    }

    private MusicProviderViewModel selectedMusicProvider;
    public MusicProviderViewModel SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set
        {
            if (IsLoading) { return; }

            if (selectedMusicProvider != null)
            {
                selectedMusicProvider.IsActiveProvider = false;
            }

            if (Set(ref selectedMusicProvider, value) && value != null)
            {
                selectedMusicProvider!.IsActiveProvider = true;
            }
        }
    }

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; }

    public ICommand LoadedCommand { get; }
    public ICommand CheckUpdatesAllCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand SyncLibraryCommand { get; }
    public ICommand OpenDownloadsFolderCommand { get; }
    public ICommand AddArtistCommand { get; }

    public MainWindowViewModel()
    {
        AddArtistCommand = new LambdaCommand(ShowNewArtistWindow, e => SelectedMusicProvider != null);
        CheckUpdatesAllCommand = new LambdaCommand(async e => await LoadItemsSource(), e => !IsLoading);
        OpenDownloadsFolderCommand = new LambdaCommand(e => FileBrowserHelper.OpenDownloadsFolder());
        SettingsCommand = new LambdaCommand(ShowSettingsWindow);
        SyncLibraryCommand = new LambdaCommand(ShowSyncLibraryWindow);
        LoadedCommand = new LambdaCommand(async e => await LoadItemsSource());
        MusicProviders = new ObservableCollection<MusicProviderViewModel>();
    }

    public MainWindowViewModel(MusicUpdateManager updateManager,
        IEnumerable<MusicProviderBase> musicProviders,
        IDbContextFactory<MusicWatcherDbContext> dbCotextFactory,
        IToastsNotifier toasts,
        IDialogWindowService dialogWindowService) : this()
    {

        this.musicProviders = musicProviders;
        this.dbCotextFactory = dbCotextFactory;
        this.toasts = toasts;
        this.dialogWindowService = dialogWindowService;
        updateManager.OnNewAlbumsFound += UpdateManager_OnNewAlbumsFound;
    }

    private void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        string toastsMessage = string.Join("\r\n", e.NewAlbums.Select((album, i) => $"{i + 1}) {album?.name ?? "<Без названия>"}"));
        toasts.ShowInformation("Найдены новые альбомы:\r\n" + toastsMessage);
    }

    private void ShowNewArtistWindow(object obj)
    {
        dialogWindowService.ShowNewArtistWindow(SelectedMusicProvider);
    }

    private void ShowSettingsWindow(object obj)
    {
        dialogWindowService.ShowSettingsWindow();

    }

    private void ShowSyncLibraryWindow(object obj)
    {
        dialogWindowService.ShowSyncLibraryWindow();
    }

    private async Task LoadItemsSource()
    {
        SelectedMusicProvider = null;
        MusicProviders.Clear();

        Dictionary<int, MusicProviderBase> dic = musicProviders.ToDictionary(i => i.Id, i => i);
        IsLoading = true;

        using var db = await dbCotextFactory.CreateDbContextAsync();

        var source = await db.MusicProviders
            .Include(p => p.Artists)
            .ToListAsync();

        foreach (var provider in source)
        {
            var providerVm = new MusicProviderViewModel()
            {
                MusicProvider = dic[provider.MusicProviderId],
                Name = provider.Name,
                Image = provider.Image,
                Uri = provider.Uri,
                MusicProviderId = provider.MusicProviderId,
                TrackedArtistsCount = provider.Artists.Count,
            };

            foreach (var artist in provider.Artists)
            {
                var artistVm = new ArtistViewModel(providerVm)
                {
                    Name = artist.Name,
                    Uri = artist.Uri,
                    Image = artist.Image,
                    ArtistId = artist.ArtistId
                };

                artistVm.OnArtistChanged += (e) => providerVm.SelectedArtist = e;
                providerVm.TrackedArtists.Add(artistVm);
            }

            providerVm.OnMusicProviderChanged += (e) => SelectedMusicProvider = e;
            MusicProviders.Add(providerVm);
        }

        IsLoading = false;
        SelectedMusicProvider = MusicProviders.LastOrDefault();
    }

}
