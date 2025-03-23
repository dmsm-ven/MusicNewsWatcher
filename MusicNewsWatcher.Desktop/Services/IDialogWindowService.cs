using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Desktop.Models.ViewModels;
using MusicNewsWatcher.Desktop.ViewModels.Windows;

namespace MusicNewsWatcher.Desktop.Services;

public interface IDialogWindowService
{
    void ShowSettingsWindow();
    void ShowSyncLibraryWindow();
    void ShowNewArtistWindow(MusicProviderViewModel musicProvider);
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
        var dialogWindow = host.Services.GetRequiredService<AddNewArtistDialog>();
        dialogWindow.DataContext = new AddNewArtistDialogViewModel(musicProvider, null);
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
