﻿using MahApps.Metro.IconPacks;
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

    string name;
    public string Name { get => name; set => Set(ref name, value); }

    public string DisplayName => Name.ToDisplayName();

    string uri;
    public string Uri { get => uri; set => Set(ref uri, value); }

    string image;
    public string Image { get => image; set => Set(ref image, value); }

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

    bool multiselectEnabled;
    public bool MultiselectEnabled
    {
        get => multiselectEnabled;
        set
        {
            if(Set(ref multiselectEnabled, value))
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

    private async Task DownloadCheckedAlbums()
    {
        foreach(var album in Albums.Where(a => a.IsChecked == true))
        {
            await album.RefreshTracksSource();
            await album.DownloadAlbum(openFolder: false);
        }
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
                album.PropertyChanged += (o, e) =>
                {
                    if(e.PropertyName == nameof(AlbumViewModel.IsChecked))
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
