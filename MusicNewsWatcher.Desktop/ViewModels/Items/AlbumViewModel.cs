using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.IconPacks;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.Models.ViewModels;

//TODO разбить/упростить класс делает слишком много лишнего
public partial class AlbumViewModel(MusicUpdateManager updateManager,
    MusicDownloadHelper downloadHelper,
    MusicWatcherDbContext dbContext,
    IToastsNotifier toasts,
    IImageThumbnailCacheService imageCacheService) : ObservableObject
{
    private bool isInitialized = false;

    public DateTime Created { get; private set; }
    public int AlbumId { get; private set; }
    public string? Image { get; private set; }
    public string Uri { get; private set; }
    public ArtistViewModel ParentArtist { get; private set; }

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private bool isActiveAlbum;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUpdateTracksButtonVisibile))]
    [NotifyPropertyChangedFor(nameof(Tracks))]
    [NotifyPropertyChangedFor(nameof(CanDownloadAlbum))]
    private bool inProgress;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentMultiselectStateIcon))]
    private bool? isChecked = null;

    public PackIconFontAwesomeKind CurrentMultiselectStateIcon
    {
        get
        {
            if (IsChecked.HasValue)
            {
                return IsChecked.Value ? PackIconFontAwesomeKind.CheckSolid : PackIconFontAwesomeKind.SquareRegular;
            }
            return PackIconFontAwesomeKind.QuestionSolid;
        }
    }

    public string CachedImage => imageCacheService.GetCachedImage(this.Image);

    public bool IsUpdateTracksButtonVisibile => Tracks.Count == 0 && !InProgress;

    private bool CanToggleMultiselectState => IsChecked.HasValue;

    public bool CanDownloadAlbum => Tracks.Count > 0 && !InProgress;

    public ObservableCollection<TrackViewModel> Tracks { get; } = new();

    private readonly CancellationTokenSource cts;

    async partial void OnIsActiveAlbumChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            await RefreshTracksSource();
        }
    }

    [RelayCommand(CanExecute = nameof(CanDownloadAlbum))]
    private async Task DownloadAlbum(bool openFolder)
    {
        await downloadHelper.DownloadAlbum(this, openFolder, cts.Token);
    }

    [RelayCommand(CanExecute = nameof(CanToggleMultiselectState))]
    private void ToggleMultiselectState()
    {
        IsChecked = (IsChecked.HasValue ? (!IsChecked.Value) : null);
    }

    private async Task RefreshTracksSource()
    {
        var tracksSource = await dbContext.Tracks.Where(t => t.AlbumId == this.AlbumId).CountAsync();

        if (tracksSource == Tracks.Count)
        {
            return;
        }


        Tracks.Clear();

        var tracksEntities = await dbContext.Tracks.Where(a => a.AlbumId == this.AlbumId).ToListAsync();

        tracksEntities
            .Select(i => new TrackViewModel(this)
            {
                AlbumId = i.AlbumId,
                Id = i.Id,
                Name = i.Name,
                DownloadUri = i.DownloadUri,
            })
            .OrderBy(a => a.Id)
            .ToList()
            .ForEach(t => Tracks.Add(t));
    }

    [RelayCommand]
    private async Task RefreshTracks()
    {
        InProgress = true;
        try
        {
            await updateManager.CheckUpdatesForAlbumAsync(ParentArtist.ParentProvider.Template, AlbumId);
            await RefreshTracksSource();
        }
        catch (Exception ex)
        {
            toasts.ShowError("Ошибка получения информации о треках" + ex.Message);
        }
        finally
        {
            InProgress = false;
        }

    }

    [RelayCommand(CanExecute = nameof(InProgress))]
    private void CancelDownloading()
    {
        toasts.ShowError("Загрузка альбома отменена");
        cts.Cancel();
    }

    public void Initialize(ArtistViewModel parentArtist, int id, string title, DateTime created, string? image, string uri)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("ViewModel already initialized");
        }
        isInitialized = true;

        this.ParentArtist = parentArtist;
        this.AlbumId = id;
        this.Title = title;
        this.Created = created;
        this.Image = image;
        this.Uri = uri;
    }
}
