using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;
using MusicNewsWatcher.Desktop.ViewModels.Base;
using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MusicNewsWatcher.Desktop.Models.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly IToastsNotifier toasts;
    private readonly MusicUpdateManager updateManager;

    public event Action<ArtistViewModel> OnArtistChanged;

    public int ArtistId { get; init; }

    private string name;
    public string Name { get => name; set => Set(ref name, value); }

    public string DisplayName => Name.ToDisplayName();

    private string uri;
    public string Uri { get => uri; set => Set(ref uri, value); }

    private string image;
    public string Image { get => image; set => Set(ref image, value); }

    private string cachedImage;
    public string CachedImage
    {
        get => cachedImage ??= GetCachedImageAndCreate(Image);
    }

    public DateTime LastAlbumDate
    {
        get
        {
            if (Albums?.Any() ?? false)
            {
                return Albums.Max(a => a.Created);
            }
            return DateTime.MinValue;
        }
    }

    private bool multiselectEnabled;
    public bool MultiselectEnabled
    {
        get => multiselectEnabled;
        set
        {
            if (Set(ref multiselectEnabled, value))
            {
                Albums.ToList().ForEach(a => a.IsChecked = value ? false : null);
                RaisePropertyChanged(nameof(CurrentMultiselectIcon));
            }
        }
    }

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

    private bool isActiveArtist;
    public bool IsActiveArtist
    {
        get => isActiveArtist;
        set => Set(ref isActiveArtist, value);
    }

    public bool IsUpdateAlbumsButtonVisibile
    {
        get => Albums.Count == 0 && !InProgress;
    }

    private bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set
        {
            if (Set(ref inProgress, value))
            {
                RaisePropertyChanged(nameof(IsUpdateAlbumsButtonVisibile));
            }
        }
    }

    private AlbumViewModel selectedAlbum;
    public AlbumViewModel SelectedAlbum
    {
        get => selectedAlbum;
        set
        {
            if (selectedAlbum != null)
            {
                selectedAlbum.IsActiveAlbum = false;
            }

            if (Set(ref selectedAlbum, value) && value != null)
            {
                selectedAlbum!.IsActiveAlbum = true;
            }
        }
    }

    public ObservableCollection<AlbumViewModel> Albums { get; init; } = new();

    public ICommand GetAlbumsFromProviderForArtistCommand { get; }
    public ICommand ArtistChangedCommand { get; }
    public ICommand ToggleMultiselectModeCommand { get; }
    public ICommand DownloadCheckedAlbumsCommand { get; }

    public MusicProviderViewModel ParentProvider { get; }

    public ArtistViewModel()
    {
        DownloadCheckedAlbumsCommand = new LambdaCommand(async e => await DownloadCheckedAlbums(), e => HasCheckedAlbums);
        ToggleMultiselectModeCommand = new LambdaCommand(e => MultiselectEnabled = !MultiselectEnabled);
        GetAlbumsFromProviderForArtistCommand = new LambdaCommand(async e => await GetAlbumsFromProviderForArtist());
        ArtistChangedCommand = new LambdaCommand(ArtistChanged);

        updateManager = App.HostContainer.Services.GetRequiredService<MusicUpdateManager>();
        toasts = App.HostContainer.Services.GetRequiredService<IToastsNotifier>();
        dbFactory = App.HostContainer.Services.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>();
    }

    public ArtistViewModel(MusicProviderViewModel parent) : this()
    {
        ParentProvider = parent;
    }

    private async Task DownloadCheckedAlbums()
    {
        foreach (var album in Albums.Where(a => a.IsChecked == true))
        {
            await album.RefreshTracksSource();
            await album.DownloadAlbum(openFolder: false);
        }
    }

    private async Task GetAlbumsFromProviderForArtist()
    {
        InProgress = true;
        await updateManager.CheckUpdatesForArtistForProvider(ParentProvider.MusicProvider, ArtistId, Name, Uri);
        await RefreshSource();
        InProgress = false;
    }

    private async void ArtistChanged(object obj)
    {
        if (IsActiveArtist) { return; }
        OnArtistChanged?.Invoke(this);
        await RefreshSource();
    }

    private async Task RefreshSource()
    {
        InProgress = true;

        using var db = await dbFactory.CreateDbContextAsync();

        int freshParsedAlbumsCount = await db.Albums.Where(a => a.ArtistId == ArtistId).CountAsync();

        if (freshParsedAlbumsCount != Albums.Count || Albums.Count == 0)
        {
            Albums.Clear();

            List<AlbumViewModel> dbAlbums = (await db
                .Albums
                .Where(a => a.ArtistId == ArtistId)
                .OrderByDescending(a => a.Created)
                .ThenBy(a => a.AlbumId)
                .ToListAsync())
                .Select(i => new AlbumViewModel(this)
                {
                    Title = i.Title,
                    Created = i.Created,
                    Image = i.Image,
                    Uri = i.Uri,
                    AlbumId = i.AlbumId,
                })
                .ToList();

            foreach (var album in dbAlbums)
            {
                album.OnAlbumChanged += (e) => SelectedAlbum = e;
                album.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == nameof(AlbumViewModel.IsChecked))
                    {
                        RaisePropertyChanged(nameof(HasCheckedAlbums));
                    }
                };
                Albums.Add(album);
            }
        }

        InProgress = false;
    }
}
