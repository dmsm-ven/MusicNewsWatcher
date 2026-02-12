using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models.WeakReferenceMessages;
using MusicNewsWatcher.Desktop.ViewModels.Mappers;
using System.Collections.ObjectModel;
using System.Windows;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

public partial class MusicProviderViewModel : ObservableObject
{
    public string Name { get; private set; }
    public int MusicProviderId { get; private set; }
    public string Image { get; private set; }
    public string Uri { get; private set; }

    private readonly MusicNewsWatcherApiClient apiClient;
    private readonly ViewModelFactory<ArtistViewModel> artistVmFactory;
    private readonly IDialogWindowService dialogWindowService;
    private readonly IToastsNotifier toasts;

    private bool isLoaded = false;
    private bool isInitialized = false;
    private int initialTrackedArtistsCount;

    [ObservableProperty]
    private ObservableCollection<ArtistViewModel> trackedArtists = new();

    [ObservableProperty]
    private bool inProgress = false;

    [ObservableProperty]
    private bool isActiveProvider;

    async partial void OnIsActiveProviderChanged(bool oldValue, bool newValue)
    {
        if (!newValue || isLoaded)
        {
            return;
        }

        await RefreshProviderSource();
    }

    public MusicProviderViewModel(MusicNewsWatcherApiClient apiClient,
    IDialogWindowService dialogWindowService,
    IToastsNotifier toasts,
    ViewModelFactory<ArtistViewModel> artistVmFactory)
    {
        this.apiClient = apiClient;
        this.dialogWindowService = dialogWindowService;
        this.toasts = toasts;
        this.artistVmFactory = artistVmFactory;

        trackedArtists.CollectionChanged += (o, e) => OnPropertyChanged(nameof(TrackedArtistsCount));
    }

    private async Task RefreshProviderSource()
    {
        var artists = await apiClient.GetProviderArtistsAsync(MusicProviderId);

        foreach (var artist in artists.OrderBy(a => a.Name))
        {
            var artistVm = artistVmFactory.Create();
            artistVm.Initialize(this, artist);

            await App.Current.Dispatcher.InvokeAsync(() => TrackedArtists.Add(artistVm));
        }

        isLoaded = true;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditArtistCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteArtistCommand))]
    private ArtistViewModel? selectedArtist;

    private bool IsArtistSelected => SelectedArtist != null;

    public int TrackedArtistsCount => TrackedArtists.Count == 0 ? initialTrackedArtistsCount : TrackedArtists.Count;

    [RelayCommand(CanExecute = nameof(IsArtistSelected))]
    private void EditArtist()
    {
        dialogWindowService.ShowEditArtistWindow(this, SelectedArtist!);
    }

    [RelayCommand(CanExecute = nameof(IsArtistSelected))]
    private async Task DeleteArtist()
    {
        if (SelectedArtist == null) { return; }
        var dialogResult = MessageBox.Show($"Удалить '{SelectedArtist!.Name}' ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (dialogResult == MessageBoxResult.Yes)
        {
            await apiClient.DeleteArtist(SelectedArtist.ArtistId);
            this.TrackedArtists.Remove(SelectedArtist);
        }
    }

    [RelayCommand]
    private async Task ChangeSelectedProvider()
    {
        IsActiveProvider = true;

        if (!isLoaded)
        {
            await RefreshProviderSource();
        }

        WeakReferenceMessenger.Default.Send(new ProviderChangedMessage(this));
    }

    [RelayCommand]
    private async Task RefreshArtistPreviewImageToRandom()
    {
        if (SelectedArtist == null)
        {
            return;
        }
        var validAlbums = SelectedArtist.Albums.Where(a => a.Image != null && !a.Image.Contains("image-placeholder")).ToArray();
        if (validAlbums.Length == 0)
        {
            return;
        }

        var randomAlbumImage = validAlbums[Random.Shared.Next(0, validAlbums.Length)].Image;
        SelectedArtist.Image = randomAlbumImage;

        await apiClient.UpdateArtist(SelectedArtist.ToDto());
    }

    public void Initialize(MusicProviderDto provider)
    {
        if (isInitialized)
        {
            throw new Exception("MusicProviderViewModel already initialized");
        }

        Name = provider.Name;
        MusicProviderId = provider.MusicProviderId;
        Image = provider.Image;
        Uri = provider.Uri;
        initialTrackedArtistsCount = provider.TotalArtists;

        isInitialized = true;
    }
}