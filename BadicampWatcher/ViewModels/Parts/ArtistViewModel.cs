using BandcampWatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class ArtistViewModel : ViewModelBase
{
    public event Action<ArtistViewModel> OnArtistChanged;

    public int ArtistId { get; init; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public string Image { get; set; }

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

    ObservableCollection<AlbumViewModel> albums;
    public ObservableCollection<AlbumViewModel> Albums
    {
        get => albums;
        set
        {
            Set(ref albums, value);
            value.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    RaisePropertyChanged(nameof(LastAlbumDate));
                }
            };
        }
    }

    public ICommand ArtistChangedCommand { get; }

    public ArtistViewModel()
    {
        ArtistChangedCommand = new LambdaCommand(e => OnArtistChanged?.Invoke(this));
    }
}
