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
using System.Diagnostics;
using System.Web;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

//TODO разбить/упростить класс делает слишком много лишнего
public partial class AlbumViewModel : ObservableObject
{
    private bool isLoaded = false;

    public DateTime Created { get; private set; }
    public int AlbumId { get; private set; }

    // Только адрес оригинальной картинки. Сама картинка загружается лениво контролом ThumbnailImage
    [ObservableProperty]
    private string? image;

    public string Uri { get; private set; }
    public ArtistViewModel ParentArtist { get; private set; }

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isActiveAlbum;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUpdateTracksButtonVisibile))]
    [NotifyPropertyChangedFor(nameof(Tracks))]
    [NotifyPropertyChangedFor(nameof(CanDownloadAlbum))]
    [NotifyCanExecuteChangedFor(nameof(DownloadAlbumCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelDownloadingCommand))]
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

    public bool IsUpdateTracksButtonVisibile => Tracks.Count == 0 && !InProgress;

    public bool CanDownloadAlbum => Tracks.Count > 0 && !InProgress;

    private bool CanCancelDownloading => InProgress;

    public ObservableCollection<TrackViewModel> Tracks { get; } = new();

    // Источник отмены создаётся заново для каждой загрузки: после первой отмены альбом можно скачать снова
    private CancellationTokenSource? downloadCts;

    private readonly MusicNewsWatcherApiClient apiClient;
    private readonly MusicDownloadHelper downloadHelper;
    private readonly IToastsNotifier toasts;

    public AlbumViewModel(ArtistViewModel parentArtist, AlbumDto dto, MusicNewsWatcherApiClient apiClient,
        MusicDownloadHelper downloadHelper,
        IToastsNotifier toasts)
    {
        this.apiClient = apiClient;
        this.downloadHelper = downloadHelper;
        this.toasts = toasts;

        ParentArtist = parentArtist;
        AlbumId = dto.AlbumId;
        Title = dto.Title.ToDisplayName();
        Created = dto.Created;
        Image = dto.Image;
        Uri = dto.Uri;
    }

    [RelayCommand(CanExecute = nameof(CanDownloadAlbum))]
    private async Task DownloadAlbum()
    {
        downloadCts?.Dispose();
        downloadCts = new CancellationTokenSource();

        await downloadHelper.DownloadAlbum(this, downloadCts.Token);
    }

    [RelayCommand]
    private void ToggleMultiselectState()
    {
        IsChecked = IsChecked.HasValue ? !((bool)IsChecked.Value) : null;
    }

    [RelayCommand]
    private void OpenInBrowser(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            toasts.ShowError("Не удалось открыть альбом: ссылка отсутствует.");
            return;
        }

        if (!System.Uri.TryCreate(uri, UriKind.Absolute, out var albumUri))
        {
            toasts.ShowError("Не удалось открыть альбом: ссылка имеет неверный формат.");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = albumUri.AbsoluteUri,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            toasts.ShowError($"Не удалось открыть альбом в браузере: {ex.Message}");
        }
    }

    private async Task RefreshTracksSource()
    {
        InProgress = true;
        try
        {
            var tracksEntities = await apiClient.GetAlbumTracksAsync(
                albumId: AlbumId,
                providerId: this.ParentArtist.ParentProvider.MusicProviderId);

            Tracks.Clear();

            foreach (var track in tracksEntities.OrderBy(t => t.Id))
            {
                Tracks.Add(new TrackViewModel(this)
                {
                    AlbumId = track.AlbumId,
                    Id = track.Id,
                    Name = HttpUtility.HtmlDecode(track.Name),
                    DownloadUri = track.DownloadUri
                });
            }

            isLoaded = true;
        }
        catch (Exception ex)
        {
            toasts.ShowError("Ошибка получения информации о треках: " + ex.Message);
        }
        finally
        {
            InProgress = false;
        }
    }

    [RelayCommand]
    private async Task RefreshTracks()
    {
        await RefreshTracksSource();
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

    [RelayCommand(CanExecute = nameof(CanCancelDownloading))]
    private void CancelDownloading()
    {
        // Уведомление об отмене показывает MusicDownloadHelper
        downloadCts?.Cancel();
    }
}
