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
    private readonly IImageThumbnailCacheService imageCacheService;
    private readonly MusicDownloadHelper downloadHelper;
    private readonly MusicNewsWatcherApiClient apiClient;
    private readonly ViewModelFactory<AlbumViewModel> albumViewFactory;

    public ArtistViewModel(IToastsNotifier toasts,
        IImageThumbnailCacheService imageCacheService,
        MusicDownloadHelper downloadHelper,
        MusicNewsWatcherApiClient apiClient,
        ViewModelFactory<AlbumViewModel> albumViewFactory)
    {
        this.toasts = toasts;
        this.imageCacheService = imageCacheService;
        this.downloadHelper = downloadHelper;
        this.apiClient = apiClient;
        this.albumViewFactory = albumViewFactory;
    }

    private bool isInitialized = false;
    private bool isLoaded = false;

    public int ArtistId { get; private set; }
    public MusicProviderViewModel ParentProvider { get; private set; }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string uri;

    [ObservableProperty]
    private string image;

    async partial void OnImageChanged(string oldValue, string newValue)
    {
        CachedImage = await imageCacheService.GetCachedImage(newValue, ThumbnailSize.Artist);
        await App.Current.Dispatcher.InvokeAsync(() => OnPropertyChanged(nameof(CachedImage)));
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
    private AlbumViewModel selectedAlbum;

    public bool IsUpdateAlbumsButtonVisibile => Albums.Count == 0 && !InProgress;

    public string? CachedImage { get; private set; }

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
        var checkedAlbums = Albums.Where(a => a?.IsChecked ?? false);
        await downloadHelper.DownloadCheckedAlbums(checkedAlbums);
    }

    [RelayCommand]
    private void ToggleMultiselectMode()
    {
        MultiselectEnabled = !MultiselectEnabled;
        this.Albums.Where(a => a.IsChecked is null).ToList().ForEach(a => a.IsChecked = false);
    }

    [RelayCommand]
    private async Task GetAlbumsFromProviderForArtist()
    {
        InProgress = true;
        await apiClient.CheckUpdatesForArtist(ArtistId);
        await RefreshSource();
        InProgress = false;
    }

    [RelayCommand]
    private void OpenInBrowser()
    {
        //IGNORE
    }

    private async Task RefreshSource()
    {
        InProgress = true;

        var albumsData = await apiClient.GetArtistAlbumsAsync(this.ArtistId);

        foreach (var albumDto in albumsData.OrderByDescending(ae => ae.Created))
        {
            var album = albumViewFactory.Create();
            album.PropertyChanged += Album_PropertyChanged;
            album.Initialize(this, albumDto);
            Albums.Add(album);
        }

        InProgress = false;
        isLoaded = true;
    }
    private void Album_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var album = (AlbumViewModel)sender!;

        if (e.PropertyName == nameof(AlbumViewModel.IsChecked))
        {
            HasCheckedAlbums = this.Albums.Any(a => a.IsChecked == true);
        }
    }

    public void Initialize(MusicProviderViewModel parentProvider, ArtistDto artist)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("ViewModel already initialized");
        }

        isInitialized = true;

        ParentProvider = parentProvider;
        ArtistId = artist.ArtistId;
        Name = artist.Name;
        Image = artist.Image;
        Uri = artist.Uri;

        this.Albums.CollectionChanged += async (o, e) => await App.Current.Dispatcher.InvokeAsync(() => OnPropertyChanged(nameof(IsUpdateAlbumsButtonVisibile)));
    }

    [RelayCommand]
    private async Task RefreshArtistAlbums()
    {
        await apiClient.CheckUpdatesForArtist(this.ArtistId);
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
