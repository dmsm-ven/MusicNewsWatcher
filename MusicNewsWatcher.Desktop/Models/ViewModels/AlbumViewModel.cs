using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Core.Extensions;
using MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;
using MusicNewsWatcher.Desktop.ViewModels.Base;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.Models.ViewModels;

//TODO разбить/упростить класс делает слишком много лишнего
public class AlbumViewModel : ViewModelBase
{
    public event Action<AlbumViewModel> OnAlbumChanged;

    private readonly MusicUpdateManager updateManager;
    private readonly MusicDownloadHelper downloadHelper;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly IToastsNotifier toasts;

    private string title;
    public string Title
    {
        get => title;
        set
        {
            if (Set(ref title, value))
            {
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
    }

    public string DisplayName => Title.ToDisplayName();

    private string cachedImage;
    public string CachedImage
    {
        get => cachedImage ??= GetCachedImageAndCreate(Image);
    }

    public DateTime Created { get; init; }
    public int AlbumId { get; init; }

    private bool isActiveAlbum;
    public bool IsActiveAlbum
    {
        get => isActiveAlbum;
        set => Set(ref isActiveAlbum, value);
    }

    public bool CanDownloadAlbum
    {
        get => Tracks.Count > 0 && !InProgress;
    }

    private bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set
        {
            if (Set(ref inProgress, value))
            {
                RaisePropertyChanged(nameof(IsUpdateTracksButtonVisibile));
                RaisePropertyChanged(nameof(Tracks));
                RaisePropertyChanged(nameof(CanDownloadAlbum));
            }
        }
    }

    private bool? isChecked = null;
    public bool? IsChecked
    {
        get => isChecked;
        set
        {
            if (Set(ref isChecked, value))
            {
                RaisePropertyChanged(nameof(CurrentMultiselectStateIcon));
            }
        }
    }

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

    public bool IsUpdateTracksButtonVisibile
    {
        get => Tracks.Count == 0 && !InProgress;
    }

    public string? Image { get; set; }
    public string Uri { get; set; }

    public ObservableCollection<TrackViewModel> Tracks { get; } = new();

    public ICommand RefreshTracksCommand { get; }
    public ICommand AlbumChangedCommand { get; }
    public ICommand DownloadAlbumCommand { get; }
    public ICommand CancelDownloadingCommand { get; }
    public ICommand ToggleMultiselectStateCommand { get; }
    public ArtistViewModel ParentArtist { get; }

    private readonly CancellationTokenSource cts;

    public AlbumViewModel()
    {
        //TODO: заменить на внедрение через параметры ctor

        downloadHelper = App.HostContainer.Services.GetRequiredService<MusicDownloadHelper>();
        updateManager = App.HostContainer.Services.GetRequiredService<MusicUpdateManager>();
        toasts = App.HostContainer.Services.GetRequiredService<IToastsNotifier>();
        dbFactory = App.HostContainer.Services.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>();

        ToggleMultiselectStateCommand = new LambdaCommand(e => IsChecked = (IsChecked.HasValue ? (!IsChecked.Value) : null), e => IsChecked.HasValue);
        CancelDownloadingCommand = new LambdaCommand(CancelDownloading, e => InProgress);
        DownloadAlbumCommand = new LambdaCommand(async e => await DownloadAlbum(openFolder: true), e => CanDownloadAlbum);
        AlbumChangedCommand = new LambdaCommand(AlbumChanged);
        Tracks.CollectionChanged += (o, e) => RaisePropertyChanged(nameof(IsUpdateTracksButtonVisibile));

        RefreshTracksCommand = new LambdaCommand(async e => await GetTracksFromProvider());

        cts = new CancellationTokenSource();
    }

    public AlbumViewModel(ArtistViewModel parent) : this()
    {
        ParentArtist = parent;
    }

    private async Task DownloadAlbum(bool openFolder)
    {
        await downloadHelper.DownloadAlbum(this, openFolder, cts.Token);
    }

    private async Task RefreshTracksSource()
    {
        using var db = await dbFactory.CreateDbContextAsync();
        var tracksSource = await db.Tracks.Where(t => t.AlbumId == this.AlbumId).CountAsync();

        if (tracksSource == Tracks.Count)
        {
            return;
        }


        Tracks.Clear();

        var tracksEntities = await db.Tracks.Where(a => a.AlbumId == this.AlbumId).ToListAsync();

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

    private async Task GetTracksFromProvider()
    {
        InProgress = true;
        try
        {
            await updateManager.CheckUpdatesForAlbumAsync(ParentArtist.ParentProvider.MusicProvider, AlbumId);
            await RefreshTracksSource();
        }
        catch (Exception ex)
        {
            toasts.ShowError("Ошибка получения информации о треках" + ex.Message);
        }
        InProgress = false;
    }

    private async void AlbumChanged(object obj)
    {
        if (IsActiveAlbum) { return; }

        OnAlbumChanged?.Invoke(this);
        await RefreshTracksSource();
    }

    private void CancelDownloading(object obj)
    {
        toasts.ShowError("Загрузка альбома отменена");
        cts.Cancel();
    }
}
