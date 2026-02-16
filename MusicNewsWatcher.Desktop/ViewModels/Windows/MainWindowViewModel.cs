using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models;
using MusicNewsWatcher.Desktop.Models.WeakReferenceMessages;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using System.Collections.ObjectModel;
using System.IO;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject,
    IRecipient<ProviderChangedMessage>,
    IRecipient<ArtistChangedMessage>,
    IRecipient<AlbumChangedMessage>

{
    private readonly MusicNewsWatcherApiClient apiClient;
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

    public MainWindowViewModel(MusicNewsWatcherApiClient apiClient,
        IToastsNotifier toasts,
        IDialogWindowService dialogWindowService,
        IOptions<MusicDownloadFolderOptions> options,
        ViewModelFactory<MusicProviderViewModel> providerVmFactory)
    {
        this.apiClient = apiClient;
        this.toasts = toasts;
        this.dialogWindowService = dialogWindowService;
        this.options = options;
        this.providerVmFactory = providerVmFactory;

        WeakReferenceMessenger.Default.Register<ProviderChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<ArtistChangedMessage>(this);
        WeakReferenceMessenger.Default.Register<AlbumChangedMessage>(this);
    }

    [RelayCommand]
    private async Task AddArtist()
    {
        dialogWindowService?.ShowNewArtistWindow(SelectedMusicProvider!);
        await CheckUpdatesAll();

    }

    [RelayCommand]
    private void OpenDownloadsFolder()
    {
        if (Directory.Exists(options.Value.MusicDownloadFolder))
        {
            FileBrowserHelper.OpenFolderInFileBrowser(options.Value.MusicDownloadFolder);
        }
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

        IsLoading = true;
        var providers = await apiClient.GetProvidersAsync();
        foreach (var provider in providers)
        {
            var providerVm = providerVmFactory.Create();
            providerVm.Initialize(provider);
            MusicProviders.Add(providerVm);
        }

        IsLoading = false;

        if (MusicProviders.Any())
        {
            SelectedMusicProvider = MusicProviders.Last();
            SelectedMusicProvider.IsActiveProvider = true;
        }
    }

    [RelayCommand]
    private void OpenFilesDownloadHistory()
    {
        dialogWindowService.ShowDownloadHistoryWindow();
    }

    public void Receive(AlbumChangedMessage message)
    {
        SelectedMusicProvider!.SelectedArtist!.SelectedAlbum = message.album;

        foreach (var album in SelectedMusicProvider.SelectedArtist.Albums)
        {
            album.IsActiveAlbum = album == message.album ? message.album.IsActiveAlbum : false;
        }
    }

    public void Receive(ArtistChangedMessage message)
    {
        SelectedMusicProvider!.SelectedArtist = message.artist;

        foreach (var artist in SelectedMusicProvider.TrackedArtists)
        {
            artist.IsActiveArtist = artist == message.artist ? message.artist.IsActiveArtist : false;
        }
    }

    public void Receive(ProviderChangedMessage message)
    {
        SelectedMusicProvider = message.provider;
        foreach (var provider in MusicProviders)
        {
            provider.IsActiveProvider = provider == SelectedMusicProvider;
        }
    }
}
