using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Core.Extensions;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace MusicNewsWatcher.Desktop.Models.ViewModels;

//TODO разбить/упростить, класс делает слишком много лишнего
public partial class ArtistViewModel(MusicWatcherDbContext dbContext,
    IToastsNotifier toasts,
    IImageThumbnailCacheService imageThumbnailCacheService,
    MusicUpdateManager updateManager,
    MusicDownloadHelper downloadHelper,
    ViewModelFactory<AlbumViewModel> albumViewFactory) : ObservableObject
{
    private bool isInitialized = false;

    public int ArtistId { get; private set; }
    public MusicProviderViewModel ParentProvider { get; private set; }

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string uri;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CachedImage))]
    private string image;

    [ObservableProperty]
    private bool multiselectEnabled;

    [ObservableProperty]
    private bool isActiveArtist;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUpdateAlbumsButtonVisibile))]
    private bool inProgress;

    [ObservableProperty]
    private AlbumViewModel selectedAlbum;

    public bool IsUpdateAlbumsButtonVisibile
    {
        get => Albums.Count == 0 && !InProgress;
    }

    public string CachedImage => imageThumbnailCacheService.GetCachedImage(Image);

    public bool HasCheckedAlbums
    {
        get => Albums.Count(a => a.IsChecked == true) >= 1;
    }

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

        int freshParsedAlbumsCount = await dbContext.Albums.Where(a => a.ArtistId == ArtistId).CountAsync();

        if (freshParsedAlbumsCount != Albums.Count || Albums.Count == 0)
        {
            Albums.Clear();

            var dbAlbumsData = (await dbContext
                .Albums
                .Where(a => a.ArtistId == ArtistId)
                .OrderByDescending(a => a.Created)
                .ThenBy(a => a.AlbumId)
                .ToListAsync())
                .Select(i => i)
                .ToList();

            foreach (var albumEntity in dbAlbumsData)
            {
                var album = albumViewFactory.Create();
                album.PropertyChanged += Album_PropertyChanged;
                Albums.Add(album);
                album.Initialize(this,
                    albumEntity.AlbumId,
                    albumEntity.Title.ToDisplayName(),
                    albumEntity.Created,
                    albumEntity.Image,
                    albumEntity.Uri);
            }
        }

        InProgress = false;
    }

    private void Album_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var album = (AlbumViewModel)sender!;

        if (e.PropertyName == nameof(AlbumViewModel.IsActiveAlbum) && album.IsActiveAlbum)
        {
            this.Albums.Where(a => a != album).ToList().ForEach(a => a.IsActiveAlbum = false);
        }

        if (e.PropertyName == nameof(AlbumViewModel.IsChecked))
        {
            OnPropertyChanged(nameof(HasCheckedAlbums));
        }
    }

    public void Initialize(MusicProviderViewModel musicProviderViewModel, int artistId, string name, string image, string uri)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("ViewModel already initialized");
        }

        isInitialized = true;

        this.ParentProvider = musicProviderViewModel;
        this.ArtistId = artistId;
        this.Name = name;
        this.Image = image;
        this.Uri = uri;
    }
}
