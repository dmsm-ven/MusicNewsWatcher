using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.ApiClient;
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
        var dialogWindow = host.Services.GetRequiredService<ArtistAddWindow>();
        var apiClient = host.Services.GetRequiredService<MusicNewsWatcherApiClient>();
        dialogWindow.DataContext = new ArtistAddWindowViewModel(musicProvider, apiClient);
        dialogWindow.ShowDialog();
    }

    public void ShowEditArtistWindow(MusicProviderViewModel musicProvider, ArtistViewModel artistViewModel)
    {
        var dialogWindow = host.Services.GetRequiredService<ArtistAddWindow>();
        var apiClient = host.Services.GetRequiredService<MusicNewsWatcherApiClient>();

        var vm = new ArtistAddWindowViewModel(musicProvider, apiClient)
        {
            ContextArtist = artistViewModel,
            IsEdit = true
        };
        dialogWindow.DataContext = vm;
        dialogWindow.ShowDialog();
    }
}
