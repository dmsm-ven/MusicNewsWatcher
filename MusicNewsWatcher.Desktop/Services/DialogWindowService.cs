using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.Interfaces;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewsWatcher.Desktop.ViewModels.Windows;

namespace MusicNewsWatcher.Desktop.Services;

public class DialogWindowService(IServiceScopeFactory scopeFactory) : IDialogWindowService
{
    public void ShowNewArtistWindow(MusicProviderViewModel musicProvider)
    {
        // ShowDialog блокирует до закрытия окна, поэтому scope безопасно освободить сразу после него
        using var scope = scopeFactory.CreateScope();

        var dialogWindow = scope.ServiceProvider.GetRequiredService<ArtistAddWindow>();
        var apiClient = scope.ServiceProvider.GetRequiredService<MusicNewsWatcherApiClient>();
        var vmFactory = scope.ServiceProvider.GetRequiredService<Func<MusicProviderViewModel, ArtistDto, ArtistViewModel>>();

        var vm = new ArtistAddWindowViewModel(musicProvider, apiClient, vmFactory(musicProvider, new()
        {
            MusicProviderId = musicProvider.MusicProviderId,
            Name = "Новый исполнитель"
        }));

        dialogWindow.DataContext = vm;
        dialogWindow.ShowDialog();
    }

    public void ShowEditArtistWindow(MusicProviderViewModel musicProvider, ArtistViewModel artistViewModel)
    {
        using var scope = scopeFactory.CreateScope();

        var dialogWindow = scope.ServiceProvider.GetRequiredService<ArtistAddWindow>();
        var apiClient = scope.ServiceProvider.GetRequiredService<MusicNewsWatcherApiClient>();

        var vm = new ArtistAddWindowViewModel(musicProvider, apiClient, artistViewModel);

        dialogWindow.DataContext = vm;
        dialogWindow.ShowDialog();
    }

    public void ShowDownloadHistoryWindow()
    {
        using var scope = scopeFactory.CreateScope();

        var windowVm = scope.ServiceProvider.GetRequiredService<DownloadHistoryWindowViewModel>();
        var window = new DownloadHistoryWindow();
        window.DataContext = windowVm;
        window.ShowDialog();
    }
}
