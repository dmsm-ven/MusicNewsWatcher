using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject,
    IRecipient<ArtistChangedMessage>,
    IRecipient<AlbumChangedMessage>
{
    private readonly MusicUpdateManager updateManager;
    private readonly IEnumerable<MusicProviderBase> musicProviderTemplates;
    private readonly MusicWatcherDbContext dbContext;
    private readonly IToastsNotifier toasts;
    private readonly IDialogWindowService dialogWindowService;
    private readonly IOptions<MusicDownloadFolderOptions> options;
    private readonly ViewModelFactory<MusicProviderViewModel> providerVmFactory;

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private MusicProviderViewModel? selectedMusicProvider;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddArtistCommand))]
    private ObservableCollection<MusicProviderViewModel> musicProviders = new();

    public bool HasSelectedProvider => SelectedMusicProvider != null;

    public MainWindowViewModel(MusicUpdateManager updateManager,
        IEnumerable<MusicProviderBase> musicProviderTemplates,
        MusicWatcherDbContext dbContext,
        IToastsNotifier toasts,
        IDialogWindowService dialogWindowService,
        IOptions<MusicDownloadFolderOptions> options,
        ViewModelFactory<MusicProviderViewModel> providerVmFactory)
    {
        this.updateManager = updateManager;
        this.musicProviderTemplates = musicProviderTemplates;
        this.dbContext = dbContext;
        this.toasts = toasts;
        this.dialogWindowService = dialogWindowService;
        this.options = options;
        this.providerVmFactory = providerVmFactory;

        WeakReferenceMessenger.Default.Register<ArtistChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<AlbumChangedMessage>(this);
    }

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

        if (MusicProviders.Any())
        {
            SelectedMusicProvider = MusicProviders.LastOrDefault();
            SelectedMusicProvider.IsActiveProvider = true;
        }
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

    public void Receive(AlbumChangedMessage message)
    {
        SelectedMusicProvider.SelectedArtist.SelectedAlbum = message.album;

        foreach (var album in SelectedMusicProvider.SelectedArtist.Albums)
        {
            album.IsActiveAlbum = album == message.album ? message.album.IsActiveAlbum : false;
        }
    }

    public void Receive(ArtistChangedMessage message)
    {
        SelectedMusicProvider.SelectedArtist = message.artist;

        foreach (var artist in SelectedMusicProvider.TrackedArtists)
        {
            artist.IsActiveArtist = artist == message.artist ? message.artist.IsActiveArtist : false;
        }
    }
}
