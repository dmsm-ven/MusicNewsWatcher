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
    private readonly Func<ArtistViewModel, AlbumDto, AlbumViewModel> albumViewFactory;

    public int ArtistId { get; init; }
    public MusicProviderViewModel ParentProvider { get; init; }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string uri;

    [ObservableProperty]
    private string image;

    private bool isLoaded = false;

    public ArtistViewModel(MusicProviderViewModel parentProvider, ArtistDto artist,
        IToastsNotifier toasts,
        IImageThumbnailCacheService imageCacheService,
        MusicDownloadHelper downloadHelper,
        MusicNewsWatcherApiClient apiClient,
        Func<ArtistViewModel, AlbumDto, AlbumViewModel> albumViewFactory)
    {
        this.toasts = toasts;
        this.imageCacheService = imageCacheService;
        this.downloadHelper = downloadHelper;
        this.apiClient = apiClient;
        this.albumViewFactory = albumViewFactory;

        ParentProvider = parentProvider;
        ArtistId = artist.ArtistId;
        Name = artist.Name;
        Image = artist.Image;
        Uri = artist.Uri;

        this.Albums.CollectionChanged += async (o, e) => await App.Current.Dispatcher.InvokeAsync(() => OnPropertyChanged(nameof(IsUpdateAlbumsButtonVisibile)));
    }


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
            var album = albumViewFactory(this, albumDto);
            album.PropertyChanged += Album_PropertyChanged;
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
