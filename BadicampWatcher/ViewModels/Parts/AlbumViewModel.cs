using BandcampWatcher.DataAccess;
using BandcampWatcher.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class AlbumViewModel : ViewModelBase
{
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
    public DateTime Created { get; set; }

    bool isViewed;

    public bool IsViewed
    {
        get => isViewed;
        set => Set(ref isViewed, value);
    }
    public string? Image { get; set; }
    public string Uri { get; set; }

    public ObservableCollection<string> Tracks { get; }

    public ICommand OpenInBrowserCommand { get; }

    public AlbumViewModel()
    {
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
