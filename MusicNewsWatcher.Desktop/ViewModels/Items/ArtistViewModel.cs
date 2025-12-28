using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.IconPacks;
using MusicNewsWatcher.BL;
using MusicNewsWatcher.Core.Extensions;
using MusicNewsWatcher.Desktop.ViewModels.Windows;
using System.Collections.ObjectModel;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;
//TODO разбить/упростить, класс делает слишком много лишнего
public partial class ArtistViewModel : ObservableObject
{
    private readonly MusicWatcherDbContext dbContext;
    private readonly IToastsNotifier toasts;
    private readonly IImageThumbnailCacheService imageCacheService;
    private readonly MusicUpdateManager updateManager;
    private readonly MusicDownloadHelper downloadHelper;
    private readonly ViewModelFactory<AlbumViewModel> albumViewFactory;

    public ArtistViewModel(MusicWatcherDbContext dbContext,
        IToastsNotifier toasts,
        IImageThumbnailCacheService imageCacheService,
        MusicUpdateManager updateManager,
        MusicDownloadHelper downloadHelper,
        ViewModelFactory<AlbumViewModel> albumViewFactory)
    {
        this.dbContext = dbContext;
        this.toasts = toasts;
        this.imageCacheService = imageCacheService;
        this.updateManager = updateManager;
        this.downloadHelper = downloadHelper;
        this.albumViewFactory = albumViewFactory;
    }

    public static ArtistViewModel Create(ArtistViewModel template, string name, string image, string uri)
    {
        return new ArtistViewModel(template.dbContext,
            template.toasts,
            template.imageCacheService,
            template.updateManager,
            template.downloadHelper,
            template.albumViewFactory)
        {
            ArtistId = template.ArtistId,
            ParentProvider = template.ParentProvider,
            Name = name,
            Image = image,
            Uri = uri
        };
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
        await updateManager.CheckUpdatesForArtistForProvider(ParentProvider.Template, ArtistId, Name, Uri);
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

        var albumsData = dbContext
            .Artists
            .Include("Albums")
            .FirstOrDefault(a => a.ArtistId == ArtistId)
            .Albums
            .ToList();

        foreach (var albumEntity in albumsData.OrderByDescending(ae => ae.Created))
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
    private async Task RefreshArtistAlbums()
    {
        var albumsToDelete = dbContext.Albums.Where(a => a.ArtistId == this.ArtistId).ToArray();
        dbContext.Albums.RemoveRange(albumsToDelete);
        await dbContext.SaveChangesAsync();
        Albums.Clear();

        await Task.Delay(TimeSpan.FromSeconds(1));
        await GetAlbumsFromProviderForArtist();
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
