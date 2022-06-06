using BandcampWatcher.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BandcampWatcher.Models;

public class ArtistModel : ViewModelBase
{
    public string Name { get; set; }
    public string Uri { get; set; }
    public string Image { get; set; }

    public DateTime LastAlbumDate => Albums?.Max(a => a.Created) ?? DateTime.MinValue;

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

    ObservableCollection<AlbumModel> albums;
    public ObservableCollection<AlbumModel> Albums
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
}
