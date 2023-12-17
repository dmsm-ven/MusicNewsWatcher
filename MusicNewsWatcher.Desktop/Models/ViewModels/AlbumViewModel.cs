using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewWatcher.BL;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.Models.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly MusicUpdateManager updateManager;

    private readonly IMusicDownloadManager downloadManager;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly IToastsNotifier toasts;

    public event Action<AlbumViewModel> OnAlbumChanged;

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
        get => cachedImage ??= GetCachedImage(Image);
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
                return IsChecked.Value ? PackIconFontAwesomeKind.CheckSquareRegular : PackIconFontAwesomeKind.SquareRegular;
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

    private CancellationTokenSource cts;

    public AlbumViewModel()
    {
        //TODO: заменить на внедрение через параметры ctor
        updateManager = App.HostContainer.Services.GetRequiredService<MusicUpdateManager>();
        downloadManager = App.HostContainer.Services.GetRequiredService<IMusicDownloadManager>();
        toasts = App.HostContainer.Services.GetRequiredService<IToastsNotifier>();
        dbFactory = App.HostContainer.Services.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>();

        ToggleMultiselectStateCommand = new LambdaCommand(e => IsChecked = !IsChecked.Value, e => IsChecked.HasValue);
        CancelDownloadingCommand = new LambdaCommand(CancelDownloading, e => InProgress);
        DownloadAlbumCommand = new LambdaCommand(async e => await DownloadAlbum(openFolder: true), e => CanDownloadAlbum);
        AlbumChangedCommand = new LambdaCommand(AlbumChanged);
        Tracks.CollectionChanged += (o, e) => RaisePropertyChanged(nameof(IsUpdateTracksButtonVisibile));

        RefreshTracksCommand = new LambdaCommand(async e => await GetTracksFromProvider());
    }

    public AlbumViewModel(ArtistViewModel parent) : this()
    {
        ParentArtist = parent;
    }

    public async Task DownloadAlbum(bool openFolder)
    {
        InProgress = true;

        Stopwatch sw = Stopwatch.StartNew();
        using (var db = dbFactory.CreateDbContext())
        {
            int parallelDownloads = int.Parse(db.Settings?.Find("DownloadThreadsNumber")?.Value ?? "1");

            downloadManager.ThreadLimit = parallelDownloads;
        }

        try
        {
            cts = new CancellationTokenSource();
            string albumDir = await downloadManager.DownloadFullAlbum(this.Uri, FileBrowserHelper.DownloadDirectory, cts.Token);

            if (!cts.IsCancellationRequested)
            {
                string msg = $"[{DateTime.Now.ToShortTimeString()}] Альбом '{DisplayName}' загружен за {sw.Elapsed.ToString("hh\\:mm\\:ss")}";
                toasts.ShowSuccess(msg);
            }

            await Task.Delay(TimeSpan.FromSeconds(1));

            FileBrowserHelper.OpenFolderInFileBrowser(albumDir);
        }
        catch (Exception ex)
        {
            if (!cts.IsCancellationRequested)
            {
                toasts.ShowError($"Ошибка загрузки альбома '{DisplayName}'\r\n{ex.Message}");
            }
        }
        finally
        {
            InProgress = false;
        }
    }

    public async Task RefreshTracksSource()
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
