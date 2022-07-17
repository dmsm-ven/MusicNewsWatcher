using BandcampWatcher.DataAccess;
using BandcampWatcher.Infrastructure.Helpers;
using BandcampWatcher.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    public event Action<AlbumViewModel> OnAlbumChangedCommand;

    string title;
    public string Title
    {
        get => title;
        set
        {
            if(Set(ref title, value))
            {
                RaisePropertyChanged(nameof(DisplayName));
            }
        }
    }
    public string DisplayName
    {
        get => Regex.Replace(Title, @"\s{2,}", " ").Trim();
    }
    public string CachedImage => GetCachedImage(Image);

    public DateTime Created { get; init; }

    bool isActiveAlbum;

    public bool IsActiveAlbum
    {
        get => isActiveAlbum;
        set => Set(ref isActiveAlbum, value);
    }

    bool isViewed;

    public bool IsViewed
    {
        get => isViewed;
        set => Set(ref isViewed, value);
    }
    public string? Image { get; set; }
    public string Uri { get; set; }

    public ObservableCollection<string> Tracks { get; }

    public ICommand OnChangedCommand { get; }
    public ICommand OpenInBrowserCommand { get; }

    public AlbumViewModel()
    {
        OnChangedCommand = new LambdaCommand(e => OnAlbumChangedCommand?.Invoke(this));
        OpenInBrowserCommand = new LambdaCommand(Album_OpenUrlClicked, e => true);
        Tracks = new ObservableCollection<string>()
        {
            "Track 1",
            "Track 2",
            "Track 3",
            "Track 4",
            "Track 5",
        };
    }

    private void Album_OpenUrlClicked(object o)
    {
        var psi = new System.Diagnostics.ProcessStartInfo();
        psi.UseShellExecute = true;
        psi.FileName = Uri;
        System.Diagnostics.Process.Start(psi);
    }
}
