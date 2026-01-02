using MusicNewsWatcher.Desktop.ViewModels.Items;

namespace MusicNewsWatcher.Desktop.Interfaces;

public interface IDialogWindowService
{
    void ShowNewArtistWindow(MusicProviderViewModel musicProvider);
    void ShowEditArtistWindow(MusicProviderViewModel musicProvider, ArtistViewModel artistViewModel);
}
