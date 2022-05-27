using BandcampWatcher.ViewModels;
using System;
using System.Windows.Input;

namespace BandcampWatcher.Models;

public class AlbumModel : ViewModelBase
{
    public event Action OpenUrlClicked;

    public string Title { get; set; }
    public DateTime Created { get; set; }

    bool isViewed;
    public bool IsViewed
    {
        get => isViewed;
        set => Set(ref isViewed, value);
    }
    public string? Image { get; set; }
    public string Uri { get; set; }


    public ICommand OpenInBrowserCommand { get; }

    public AlbumModel()
    {
        OpenInBrowserCommand = new LambdaCommand(e => OpenUrlClicked?.Invoke(), e => true);
    }
}
