using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicNewsWatcher.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private readonly MusicManager manager;
    private readonly MusicProviderBase provider;

    public event Action<ArtistViewModel> OnArtistChanged;

    public int ArtistId { get; init; }
    public int MusicProviderId { get; init; }

    public string Name { get; set; }
    public string Uri { get; set; }
    public string Image { get; set; }

    public string CachedImage => GetCachedImage(Image);

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

    bool checkInProgress;

    public bool CheckInProgress
    {
        get => checkInProgress;
        set => Set(ref checkInProgress, value);
    }

    AlbumViewModel selectedAlbum;
    public AlbumViewModel SelectedAlbum
    {
        get => selectedAlbum;
        set => Set(ref selectedAlbum, value);
    }

    public ObservableCollection<AlbumViewModel> Albums { get; init; } = new();

    public ICommand ArtistChangedCommand { get; }

    public ArtistViewModel()
    {
        ArtistChangedCommand = new LambdaCommand(async e => await ArtistChanged());
    }

    public ArtistViewModel(IDbContextFactory<MusicWatcherDbContext> dbFactory, 
        MusicManager manager, 
        MusicProviderBase provider) : this()
    {      
        this.dbFactory = dbFactory;
        this.manager = manager;
        this.provider = provider;
    }

    private async Task ArtistChanged()
    {
        OnArtistChanged?.Invoke(this);
        if(Albums.Count == 0)
        {
            await RefreshSource();
        }
    }

    private async Task RefreshSource()
    {
        Albums.Clear();

        using var db = await dbFactory.CreateDbContextAsync();

        var albums = db
            .Albums.Where(a => a.ArtistId == ArtistId)
            .Select(i => new AlbumViewModel(dbFactory, manager, provider)
            {
                Title = i.Title,
                Created = i.Created,
                Image = i.Image,
                Uri = i.Uri,
                IsViewed = i.IsViewed,
                AlbumId = i.AlbumId,
            })
            .OrderByDescending(a => a.Created)
            .ToList();

        foreach (var album in albums)
        {
            album.OnAlbumChanged += (e) =>
            {
                Albums.ToList().ForEach(a => a.IsActiveAlbum = false);
                e.IsActiveAlbum = true;
                SelectedAlbum = e;
            };
            Albums.Add(album);
        }
    }
}
