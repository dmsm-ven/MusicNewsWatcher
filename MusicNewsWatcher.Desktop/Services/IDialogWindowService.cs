using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewsWatcher.Desktop.ViewModels.Windows;

namespace MusicNewsWatcher.Desktop.Services;

public interface IDialogWindowService
{
    void ShowSettingsWindow();
    void ShowSyncLibraryWindow();
    void ShowNewArtistWindow(MusicProviderViewModel musicProvider);
    void ShowEditArtistWindow(MusicProviderViewModel musicProvider, ArtistViewModel artistViewModel);
}

public class DialogWindowService : IDialogWindowService
{
    private readonly IHost host;
    private readonly MusicWatcherDbContext dbContext;

    public DialogWindowService(IHost host, MusicWatcherDbContext dbContext)
    {
        this.host = host;
        this.dbContext = dbContext;
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

    public void ShowSettingsWindow()
    {
        var window = host.Services.GetRequiredService<SettingsWindow>();
        window.DataContext = host.Services.GetRequiredService<SettingsWindowViewModel>();
        window.ShowDialog();
    }

    public void ShowSyncLibraryWindow()
    {
        var window = host.Services.GetRequiredService<SyncLibraryWindow>();
        window.DataContext = host.Services.GetRequiredService<SyncLibraryWindowViewModel>();
        window.ShowDialog();
    }
}
