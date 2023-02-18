using Microsoft.Extensions.Hosting;

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
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;

    public DialogWindowService(IHost host, IDbContextFactory<MusicWatcherDbContext> dbFactory)
    {
        this.host = host;
        this.dbFactory = dbFactory;
    }

    public void ShowNewArtistWindow(MusicProviderViewModel musicProvider)
    {
        var dialogWindow = host.Services.GetRequiredService<AddNewArtistDialog>();
        dialogWindow.DataContext = new AddNewArtistDialogViewModel(musicProvider, dbFactory);
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
