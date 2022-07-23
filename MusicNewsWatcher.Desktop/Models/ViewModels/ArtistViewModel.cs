using MusicNewsWatcher.Desktop.Services;
using MusicNewsWatcher.Infrastructure.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MusicNewsWatcher.Desktop.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory; 
    private readonly IToastsNotifier toasts;
    private readonly MusicUpdateManager updateManager;

    public event Action<ArtistViewModel> OnArtistChanged;

    public int ArtistId { get; init; }
    public string Name { get; set; }
    public string DisplayName => Name.ToDisplayName();
    public string Uri { get; set; }
    public string Image { get; set; }

    string cachedImage;
    public string CachedImage
    {
        get => cachedImage ??= GetCachedImage(Image);
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

    bool hasNew;
    public bool HasNew
    {
        get => hasNew;
        set => Set(ref hasNew, value);
    }

    bool isActiveArtist;
    public bool IsActiveArtist
    {
        get => isActiveArtist;
        set => Set(ref isActiveArtist, value);
    }

    public bool IsUpdateAlbumsButtonVisibile
    {
        get => Albums.Count == 0 && !InProgress;
    }

    bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set
        {
            if(Set(ref inProgress, value))
            {
                RaisePropertyChanged(nameof(IsUpdateAlbumsButtonVisibile));
            }
        }
    }

    AlbumViewModel selectedAlbum;
    public AlbumViewModel SelectedAlbum
    {
        get => selectedAlbum;
        set
        {
            if(selectedAlbum != null)
            {
                selectedAlbum.IsActiveAlbum = false;
            }

            if(Set(ref selectedAlbum, value) && value != null)
            {
                selectedAlbum!.IsActiveAlbum = true;
            }
        }
    }

    public ObservableCollection<AlbumViewModel> Albums { get; init; } = new();

    public ICommand GetAlbumsFromProviderForArtistCommand { get; }
    public ICommand ArtistChangedCommand { get; }

    public MusicProviderViewModel ParentProvider { get; }

    public ArtistViewModel()
    {
        GetAlbumsFromProviderForArtistCommand = new LambdaCommand(async e => await GetAlbumsFromProviderForArtist());
        ArtistChangedCommand = new LambdaCommand(ArtistChanged);

        updateManager = App.HostContainer.Services.GetRequiredService<MusicUpdateManager>();
        toasts = App.HostContainer.Services.GetRequiredService<IToastsNotifier>();
        dbFactory = App.HostContainer.Services.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>();
    }

    private async Task GetAlbumsFromProviderForArtist()
    {
        InProgress = true;
        await updateManager.CheckUpdatesForArtistAsync(ParentProvider.MusicProvider, ArtistId);
        await RefreshSource();
        InProgress = false;
    }

    private async void ArtistChanged(object obj)
    {
        if (IsActiveArtist) { return; }
        OnArtistChanged?.Invoke(this);
        await RefreshSource();
    }

    public ArtistViewModel(MusicProviderViewModel parent) : this()
    {
        ParentProvider = parent;
    }

    private async Task RefreshSource()
    {
        InProgress = true;

        using var db = await dbFactory.CreateDbContextAsync();

        if (await db.Albums.Where(a => a.ArtistId == ArtistId).CountAsync() != Albums.Count)
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
                Albums.Add(album);
            }
        }

        InProgress = false;
    }
}
