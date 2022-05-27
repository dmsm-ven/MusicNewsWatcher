using BandcampWatcher.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BandcampWatcher.Models;

public class ArtistModel : ViewModelBase
{
    public string Name { get; set; }
    public string Uri { get; set; }
    public string Image { get; set; }

    bool hasNew;
    public bool HasNew
    {
        get => hasNew;
        set => Set(ref hasNew, value);
    }

    bool checkInProgress;
    public bool CheckInProgress
    {
        get => checkInProgress;
        set => Set(ref checkInProgress, value);
    }

    public ObservableCollection<AlbumModel> Albums { get; set; } = new ObservableCollection<AlbumModel>();
}
