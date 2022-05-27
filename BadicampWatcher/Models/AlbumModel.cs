using BandcampWatcher.DataAccess;
using BandcampWatcher.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace BandcampWatcher.Models;

public class AlbumModel : ViewModelBase
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
    private readonly BandcampWatcherDbContext dbContext;

    public bool IsViewed
    {
        get => isViewed;
        set => Set(ref isViewed, value);
    }
    public string? Image { get; set; }
    public string Uri { get; set; }

    public ICommand OpenInBrowserCommand { get; }

    public AlbumModel(BandcampWatcherDbContext dbContext)
    {
        OpenInBrowserCommand = new LambdaCommand(Album_OpenUrlClicked, e => true);
        this.dbContext = dbContext;
    }

    private void Album_OpenUrlClicked(object o)
    {
        var psi = new System.Diagnostics.ProcessStartInfo();
        psi.UseShellExecute = true;
        psi.FileName = Uri;
        System.Diagnostics.Process.Start(psi);

        var albumEntity = dbContext.Albums.Single(a => a.Uri == Uri);
        albumEntity.IsViewed = IsViewed = true;
        dbContext.SaveChanges();
    }
}
