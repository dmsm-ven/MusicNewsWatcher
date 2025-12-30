using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewsWatcher.Desktop.ViewModels.Windows;

namespace MusicNewsWatcher.Desktop.Services;

public class DialogWindowService : IDialogWindowService
{
    private readonly IHost host;

    public DialogWindowService(IHost host)
    {
        this.host = host;
    }

    public void ShowNewArtistWindow(MusicProviderViewModel musicProvider)
    {
        var dialogWindow = host.Services.GetRequiredService<AddOrEditArtistDialog>();
        dialogWindow.DataContext = new AddOrEditArtistDialogViewModel(musicProvider);
        dialogWindow.ShowDialog();
    }

    public void ShowEditArtistWindow(MusicProviderViewModel musicProvider, ArtistViewModel artistViewModel)
    {
        var dialogWindow = host.Services.GetRequiredService<AddOrEditArtistDialog>();
        var vm = new AddOrEditArtistDialogViewModel(musicProvider)
        {
            ContextArtist = artistViewModel,
            IsEdit = true
        };
        dialogWindow.DataContext = vm;
        dialogWindow.ShowDialog();
    }
}
