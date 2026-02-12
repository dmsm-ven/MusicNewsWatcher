using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.IconPacks;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.Extensions;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.Models.WeakReferenceMessages;
using System.Collections.ObjectModel;
using System.Web;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

//TODO разбить/упростить класс делает слишком много лишнего
public partial class AlbumViewModel(MusicNewsWatcherApiClient apiClient,
    MusicDownloadHelper downloadHelper,
    IToastsNotifier toasts,
    IImageThumbnailCacheService imageCacheService) : ObservableObject
{
    private bool isLoaded = false;
    private bool isInitialized = false;

    public DateTime Created { get; private set; }
    public int AlbumId { get; private set; }

    [ObservableProperty]
    private string? image;

    async partial void OnImageChanged(string? newValue)
    {
        CachedImage = await imageCacheService.GetCachedImage(newValue, ThumbnailSize.Album);
        await App.Current.Dispatcher.InvokeAsync(() => OnPropertyChanged(nameof(CachedImage)));
    }

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

    public string? CachedImage { get; private set; } = null;

    public bool IsUpdateTracksButtonVisibile => Tracks.Count == 0 && !InProgress;

    public bool CanDownloadAlbum => Tracks.Count > 0 && !InProgress;

    public ObservableCollection<TrackViewModel> Tracks { get; } = new();

    private readonly CancellationTokenSource cts = new();

    [RelayCommand(CanExecute = nameof(CanDownloadAlbum))]
    private async Task DownloadAlbum(bool openFolder)
    {
        await downloadHelper.DownloadAlbum(this, cts.Token);
    }

    [RelayCommand]
    private void ToggleMultiselectState()
    {
        IsChecked = IsChecked.HasValue ? !((bool)IsChecked.Value) : null;
    }

    [RelayCommand]
    private void OpenInBrowser()
    {
        //IGNORE
    }

    private async Task RefreshTracksSource()
    {
        InProgress = true;
        try
        {
            var tracksEntities = await apiClient.GetAlbumTracksAsync(
                albumId: AlbumId,
                providerId: this.ParentArtist.ParentProvider.MusicProviderId);

            tracksEntities
                .Select(i => new TrackViewModel(this)
                {
                    AlbumId = i.AlbumId,
                    Id = i.Id,
                    Name = HttpUtility.HtmlDecode(i.Name),
                    DownloadUri = i.DownloadUri
                })
                .OrderBy(a => a.Id)
                .ToList()
                .ForEach(t => Tracks.Add(t));


        }
        finally
        {
            isLoaded = true;
            InProgress = false;
        }
    }

    [RelayCommand]
    private async Task RefreshTracks()
    {
        InProgress = true;
        try
        {
            throw new NotImplementedException();
            //await updateManager.CheckUpdatesForAlbumAsync(ParentArtist.ParentProvider.Template, AlbumId);
            //await RefreshTracksSource();
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

    [RelayCommand]
    private async Task SelectThisAlbum()
    {
        IsActiveAlbum = true;

        if (!isLoaded)
        {
            await RefreshTracksSource();
        }

        WeakReferenceMessenger.Default.Send(new AlbumChangedMessage(this));
    }

    [RelayCommand(CanExecute = nameof(InProgress))]
    private void CancelDownloading()
    {
        toasts.ShowError("Загрузка альбома отменена");
        cts.Cancel();
    }

    public void Initialize(ArtistViewModel parentArtist, AlbumDto dto)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("ViewModel already initialized");
        }
        isInitialized = true;

        ParentArtist = parentArtist;
        AlbumId = dto.AlbumId;
        Title = dto.Title.ToDisplayName();
        Created = dto.Created;
        Image = dto.Image;
        Uri = dto.Uri;
    }
}
