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
    public string Name { get; init; }
    public int MusicProviderId { get; init; }
    public string Image { get; init; }
    public string Uri { get; init; }

    private readonly MusicNewsWatcherApiClient apiClient;
    private readonly Func<MusicProviderViewModel, ArtistDto, ArtistViewModel> artistVmFactory;
    private readonly IDialogWindowService dialogWindowService;
    private readonly IToastsNotifier toasts;

    // Загрузка исполнителей может быть запрошена сразу из нескольких мест (смена IsActiveProvider и команда выбора провайдера).
    // Блокировка гарантирует, что список запрашивается и заполняется один раз, а не дублируется.
    private readonly SemaphoreSlim loadLock = new(1, 1);

    private bool isLoaded = false;
    private int initialTrackedArtistsCount;

    [ObservableProperty]
    private ObservableCollection<ArtistViewModel> trackedArtists = new();

    [ObservableProperty]
    private bool inProgress = false;

    [ObservableProperty]
    private bool isActiveProvider;

    async partial void OnIsActiveProviderChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            await LoadArtistsAsync(force: false);
        }
    }

    public MusicProviderViewModel(MusicProviderDto dto, MusicNewsWatcherApiClient apiClient,
    IDialogWindowService dialogWindowService,
    IToastsNotifier toasts,
    Func<MusicProviderViewModel, ArtistDto, ArtistViewModel> artistVmFactory)
    {
        this.apiClient = apiClient;
        this.dialogWindowService = dialogWindowService;
        this.toasts = toasts;
        this.artistVmFactory = artistVmFactory;

        Name = dto.Name;
        MusicProviderId = dto.MusicProviderId;
        Image = dto.Image;
        Uri = dto.Uri;
        initialTrackedArtistsCount = dto.TotalArtists;

        TrackedArtists.CollectionChanged += (o, e) => OnPropertyChanged(nameof(TrackedArtistsCount));
    }

    /// <summary>
    /// Полностью перезагружает список исполнителей провайдера с сервера
    /// </summary>
    public Task ReloadArtistsAsync()
    {
        return LoadArtistsAsync(force: true);
    }

    private async Task LoadArtistsAsync(bool force)
    {
        await loadLock.WaitAsync();
        try
        {
            if (isLoaded && !force)
            {
                return;
            }

            InProgress = true;

            var artists = await apiClient.GetProviderArtistsAsync(MusicProviderId);
            var viewModels = artists
                .OrderBy(a => a.Name)
                .Select(artistDto => artistVmFactory(this, artistDto))
                .ToList();

            SelectedArtist = null;
            TrackedArtists.Clear();
            foreach (var artistVm in viewModels)
            {
                TrackedArtists.Add(artistVm);
            }

            isLoaded = true;
        }
        catch (Exception ex)
        {
            toasts.ShowError($"Не удалось загрузить исполнителей: {ex.Message}");
        }
        finally
        {
            InProgress = false;
            loadLock.Release();
        }
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
        var artist = SelectedArtist;
        if (artist == null) { return; }

        var dialogResult = MessageBox.Show($"Удалить '{artist.Name}' ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (dialogResult != MessageBoxResult.Yes) { return; }

        try
        {
            await apiClient.DeleteArtist(artist.ArtistId);
            this.TrackedArtists.Remove(artist);
            this.SelectedArtist = null;
        }
        catch (Exception ex)
        {
            toasts.ShowError($"Не удалось удалить исполнителя: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ChangeSelectedProvider()
    {
        IsActiveProvider = true;

        // Если загрузку уже запустил OnIsActiveProviderChanged, дождёмся её (блокировка) и ничего не продублируем
        await LoadArtistsAsync(force: false);

        WeakReferenceMessenger.Default.Send(new ProviderChangedMessage(this));
    }

    [RelayCommand]
    private async Task RefreshArtistPreviewImageToRandom()
    {
        var artist = SelectedArtist;
        if (artist == null)
        {
            return;
        }
        var validAlbums = artist.Albums.Where(a => a.Image != null && !a.Image.Contains("image-placeholder")).ToArray();
        if (validAlbums.Length == 0)
        {
            return;
        }

        var randomAlbumImage = validAlbums[Random.Shared.Next(0, validAlbums.Length)].Image ?? string.Empty;

        try
        {
            artist.Image = randomAlbumImage;
            await apiClient.UpdateArtist(artist.ToDto());
        }
        catch (Exception ex)
        {
            toasts.ShowError($"Не удалось обновить превью исполнителя: {ex.Message}");
        }
    }
}
