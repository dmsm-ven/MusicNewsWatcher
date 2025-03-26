using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Core.Extensions;
using MusicNewsWatcher.Desktop.ViewModels.Windows;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;
//TODO разбить/упростить, класс делает слишком много лишнего
public partial class ArtistViewModel(MusicWatcherDbContext dbContext,
    IToastsNotifier toasts,
    IImageThumbnailCacheService imageCacheService,
    MusicUpdateManager updateManager,
    MusicDownloadHelper downloadHelper,
    ViewModelFactory<AlbumViewModel> albumViewFactory) : ObservableObject
{
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
        await updateManager.CheckUpdatesForArtistForProvider(ParentProvider.Template, ArtistId, Name, Uri);
        await RefreshSource();
        InProgress = false;
    }

    private async Task RefreshSource()
    {
        InProgress = true;

        var albumsData = dbContext
            .Artists
            .Include("Albums")
            .FirstOrDefault(a => a.ArtistId == ArtistId)
            .Albums
            .ToList();

        foreach (var albumEntity in albumsData)
        {
            var album = albumViewFactory.Create();
            album.PropertyChanged += Album_PropertyChanged;
            album.Initialize(this,
                albumEntity.AlbumId,
                albumEntity.Title.ToDisplayName(),
                albumEntity.Created,
                albumEntity.Image,
                albumEntity.Uri);
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

    public void Initialize(MusicProviderViewModel musicProviderViewModel,
        int artistId, string name, string image, string uri)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("ViewModel already initialized");
        }

        isInitialized = true;

        ParentProvider = musicProviderViewModel;
        ArtistId = artistId;
        Name = name;
        Image = image;
        Uri = uri;

        this.Albums.CollectionChanged += async (o, e) => await App.Current.Dispatcher.InvokeAsync(() => OnPropertyChanged(nameof(IsUpdateAlbumsButtonVisibile)));
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
