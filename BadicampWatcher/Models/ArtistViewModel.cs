using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MusicNewsWatcher.Infrastructure.Helpers;
using ToastNotifications;
using ToastNotifications.Messages;
using System.Collections.Generic;

namespace MusicNewsWatcher.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly MusicUpdateManager manager;
    private readonly MusicProviderBase provider;
    private readonly Notifier tosts;

    public event Action<ArtistViewModel> OnArtistChanged;

    public int ArtistId { get; init; }
    public int MusicProviderId { get; init; }

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
        set => Set(ref selectedAlbum, value);
    }

    public ObservableCollection<AlbumViewModel> Albums { get; init; } = new();

    public ICommand ArtistChangedCommand { get; }
    public ICommand DownloadAlbumsCommand { get; }

    public ArtistViewModel()
    {
        ArtistChangedCommand = new LambdaCommand(async e => await ArtistChanged());
        DownloadAlbumsCommand = new LambdaCommand(async e => await DownloadAlbums());
    }

    private async Task DownloadAlbums()
    {
        InProgress = true;
        await manager.CheckUpdatesForArtistAsync(provider, ArtistId);
        await RefreshSource();
        InProgress = false;
    }

    public ArtistViewModel(IDbContextFactory<MusicWatcherDbContext> dbFactory, 
        MusicUpdateManager manager, 
        MusicProviderBase provider,
        Notifier tosts) : this()
    {      
        this.dbFactory = dbFactory;
        this.manager = manager;
        this.provider = provider;
        this.tosts = tosts;
    }

    private async Task ArtistChanged()
    {
        if (!IsActiveArtist)
        {
            OnArtistChanged?.Invoke(this);
            IsActiveArtist = true;
            await RefreshSource();
        }
    }

    //TODO: исправить утечку памяти при загрузке (превью не выгружаются из ItemsControl)
    private async Task RefreshSource()
    {
        InProgress = true;

        List<AlbumViewModel> dbAlbums = await GetMappedAlbums();

        if (Albums.Count != dbAlbums.Count)
        {
            if(Albums.Count != 0)
            {
                tosts.ShowSuccess($"Найден новый альбом исполнителя '{DisplayName}'");
            }

            Albums.ToList().ForEach(a => a.OnAlbumChanged -= Album_OnAlbumChanged);
            Albums.Clear();

            foreach (var album in dbAlbums)
            {
                album.OnAlbumChanged += Album_OnAlbumChanged;
                Albums.Add(album);
            }

            Albums.ToList().ForEach(async a => await a.InvalidateCacheImage());            
        }

        CommandManager.InvalidateRequerySuggested();
        InProgress = false;
    }

    private async Task<List<AlbumViewModel>> GetMappedAlbums()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var dbAlbums = db
            .Albums.Where(a => a.ArtistId == ArtistId)
            .Select(i => new AlbumViewModel(dbFactory, provider)
            {
                Title = i.Title,
                Created = i.Created,
                Image = i.Image,
                Uri = i.Uri,
                AlbumId = i.AlbumId,
                ParentArtistName = Name
            })
            .OrderByDescending(a => a.Created)
            .ThenBy(a => a.AlbumId)
            .ToList();

        return dbAlbums;
    }

    public void Album_OnAlbumChanged(AlbumViewModel e)
    {
        if(SelectedAlbum != null)
        {
            SelectedAlbum.IsActiveAlbum = false;
        }
        SelectedAlbum = e;

        GC.Collect(2);
    }
}
