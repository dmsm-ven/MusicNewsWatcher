using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.IconPacks;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models.WeakReferenceMessages;
using System.Collections.ObjectModel;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

public partial class ArtistViewModel : ObservableObject
{
    private readonly IToastsNotifier toasts;
    private readonly MusicDownloadHelper downloadHelper;
    private readonly MusicNewsWatcherApiClient apiClient;
    private readonly Func<ArtistViewModel, AlbumDto, AlbumViewModel> albumViewFactory;

    public int ArtistId { get; init; }
    public MusicProviderViewModel ParentProvider { get; init; }

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string uri = string.Empty;

    // Только адрес оригинальной картинки. Сама картинка загружается лениво контролом ThumbnailImage
    [ObservableProperty]
    private string image = string.Empty;

    private bool isLoaded = false;

    public ArtistViewModel(MusicProviderViewModel parentProvider, ArtistDto artist,
        IToastsNotifier toasts,
        MusicDownloadHelper downloadHelper,
        MusicNewsWatcherApiClient apiClient,
        Func<ArtistViewModel, AlbumDto, AlbumViewModel> albumViewFactory)
    {
        this.toasts = toasts;
        this.downloadHelper = downloadHelper;
        this.apiClient = apiClient;
        this.albumViewFactory = albumViewFactory;

        ParentProvider = parentProvider;
        ArtistId = artist.ArtistId;
        Name = artist.Name;
        Image = artist.Image;
        Uri = artist.Uri;

        this.Albums.CollectionChanged += (o, e) => OnPropertyChanged(nameof(IsUpdateAlbumsButtonVisibile));
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentMultiselectIcon))]
    private bool multiselectEnabled;

    [ObservableProperty]
    private bool isActiveArtist;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUpdateAlbumsButtonVisibile))]
    private bool inProgress;

    [ObservableProperty]
    private AlbumViewModel? selectedAlbum = null;

    public bool IsUpdateAlbumsButtonVisibile => Albums.Count == 0 && !InProgress;

    [NotifyCanExecuteChangedFor(nameof(DownloadCheckedAlbumsCommand))]
    [ObservableProperty]
    private bool hasCheckedAlbums = false;

    public PackIconFontAwesomeKind CurrentMultiselectIcon
    {
        get => MultiselectEnabled ?
            PackIconFontAwesomeKind.SquareRegular :
            PackIconFontAwesomeKind.CheckDoubleSolid;
    }

    [ObservableProperty]
    private ObservableCollection<AlbumViewModel> albums = new();

    [RelayCommand(CanExecute = nameof(HasCheckedAlbums))]
    private async Task DownloadCheckedAlbums()
    {
        var checkedAlbums = Albums.Where(a => a?.IsChecked ?? false).ToList();
        await downloadHelper.DownloadCheckedAlbums(checkedAlbums);
    }

    [RelayCommand]
    private void ToggleMultiselectMode()
    {
        MultiselectEnabled = !MultiselectEnabled;
        this.Albums.Where(a => a.IsChecked is null).ToList().ForEach(a => a.IsChecked = false);
    }

    /// <summary>
    /// Просит сервер проверить сайт-источник на новые альбомы и обновляет список
    /// </summary>
    [RelayCommand]
    private async Task GetAlbumsFromProviderForArtist()
    {
        await CheckUpdatesAndRefresh();
    }

    [RelayCommand]
    private async Task RefreshArtistAlbums()
    {
        await CheckUpdatesAndRefresh();
    }

    [RelayCommand]
    private void OpenInBrowser()
    {
        //IGNORE
    }

    private async Task CheckUpdatesAndRefresh()
    {
        InProgress = true;
        try
        {
            await apiClient.CheckUpdatesForArtist(ArtistId);
        }
        catch (Exception ex)
        {
            InProgress = false;
            toasts.ShowError($"Ошибка получения новинок: {ex.Message}");
            return;
        }

        await RefreshSource();
    }

    private async Task RefreshSource()
    {
        InProgress = true;

        try
        {
            var albumsData = await apiClient.GetArtistAlbumsAsync(this.ArtistId);

            foreach (var oldAlbum in Albums)
            {
                oldAlbum.PropertyChanged -= Album_PropertyChanged;
            }

            SelectedAlbum = null;
            HasCheckedAlbums = false;
            Albums.Clear();

            foreach (var albumDto in albumsData.OrderByDescending(ae => ae.Created))
            {
                var album = albumViewFactory(this, albumDto);
                album.PropertyChanged += Album_PropertyChanged;
                Albums.Add(album);
            }

            isLoaded = true;
        }
        catch (Exception ex)
        {
            toasts.ShowError($"Ошибка загрузки альбомов: {ex.Message}");
        }
        finally
        {
            InProgress = false;
        }
    }

    private void Album_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AlbumViewModel.IsChecked))
        {
            HasCheckedAlbums = this.Albums.Any(a => a.IsChecked == true);
        }
    }

    [RelayCommand]
    private async Task SelectThisArtist()
    {
        IsActiveArtist = true;

        if (!isLoaded)
        {
            await RefreshSource();
        }

        WeakReferenceMessenger.Default.Send(new ArtistChangedMessage(this));
    }
}
