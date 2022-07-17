using BandcampWatcher.DataAccess;
using BandcampWatcher.Infrastructure.Helpers;
using BandcampWatcher.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;

    public event Action<ArtistViewModel> OnArtistChanged;

    public int ArtistId { get; init; }
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

    public ArtistViewModel(IDbContextFactory<MusicWatcherDbContext> dbFactory)
    {
        ArtistChangedCommand = new LambdaCommand(async e => await ArtistChanged());
        this.dbFactory = dbFactory;
    }

    private async Task ArtistChanged()
    {
        OnArtistChanged?.Invoke(this);

        Albums.Clear();

        using var db = await dbFactory.CreateDbContextAsync();

        var albums = db
            .Albums.Where(a => a.ArtistId == ArtistId)
            .Select(i => new AlbumViewModel()
            {
                Title = i.Title,
                Created = i.Created,
                Image = i.Image,
                Uri = i.Uri,
                IsViewed = i.IsViewed,               
            })
            .OrderByDescending(a => a.Created)
            .ToList();

        foreach(var album in albums)
        {
            Albums.Add(album);
        }
    }
}
