namespace MusicNewsWatcher.Desktop.ViewModels;

public class MainWindowViewModel_DesignTime : MainWindowViewModel
{
    public MainWindowViewModel_DesignTime() : base(null, null, null, null, null, null)
    {
        this.MusicProviders.Add(new Models.ViewModels.MusicProviderViewModel()
        {
            Name = "Провайдер 1",
            TrackedArtists = new System.Collections.ObjectModel.ObservableCollection<Models.ViewModels.ArtistViewModel>()
            {
                new(),
                new(),
                new(),
                new(),
            }
        });
        this.MusicProviders.Add(new Models.ViewModels.MusicProviderViewModel()
        {
            Name = "Провайдер 2",
            TrackedArtists = new System.Collections.ObjectModel.ObservableCollection<Models.ViewModels.ArtistViewModel>()
            {
                new(),
            }
        });
    }
}
