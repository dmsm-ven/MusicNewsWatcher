using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicNewsWatcher.Desktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Messages;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IEnumerable<MusicProviderBase> musicProviders;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;
    private readonly MusicUpdateManager updateManager;
    private readonly Notifier toasts;

    public string Title
    {
        get
        {
            string title = "Music News Watcher";

            return title;
        }
    }

    bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set => Set(ref isLoading, value);
    }

    MusicProviderViewModel selectedMusicProvider;
    public MusicProviderViewModel SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set
        {
            if (Set(ref selectedMusicProvider, value))
            {
                RaisePropertyChanged(nameof(Title));
            }
        }
    }

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; } 
    
    public ICommand LoadedCommand { get; }
    public ICommand CheckUpdatesAllCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand OpenDownloadsFolderCommand { get; }

    public MainWindowViewModel()
    {
        CheckUpdatesAllCommand = new LambdaCommand(async e => await CheckUpdatesAll(), e => !IsLoading);
        OpenDownloadsFolderCommand = new LambdaCommand(e => FileBrowserHelper.OpenDownloadsFolder());       
        SettingsCommand = new LambdaCommand(ShowSettingsWindow);       
        LoadedCommand = new LambdaCommand(async e => await RefreshProviders());       
        MusicProviders = new ObservableCollection<MusicProviderViewModel>();
    }


    public MainWindowViewModel(
        IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbCotextFactory,
        MusicUpdateManager updateManager,
        Notifier toasts) : this()
    {

        this.musicProviders = musicProviders;
        this.dbCotextFactory = dbCotextFactory;
        this.updateManager = updateManager;
        this.toasts = toasts;
        updateManager.OnUpdate += UpdateManager_OnUpdate;
    }

    private void ShowSettingsWindow(object obj)
    {
        var window = App.HostContainer.Services.GetRequiredService<SettingsWindow>();
        window.DataContext = App.HostContainer.Services.GetRequiredService<SettingsWindowViewModel>();
        window.ShowDialog();
    }

    private async void UpdateManager_OnUpdate(AlbumEntity[] albums)
    {
        await App.Current.MainWindow.Dispatcher.InvokeAsync(async () => await RefreshProviders());

        string toastsMessage = string.Join(" ", albums.Select((album, i) => $"{i + 1}) {album?.Title ?? "<Без названия>"}"));
        toasts.ShowInformation("Найдены новые альбомы: " + toastsMessage);
    }

    private async Task CheckUpdatesAll()
    {
        IsLoading = true;
        await updateManager.CheckUpdatesAllAsync();
        IsLoading = false;
    }

    private async Task RefreshProviders()
    {
        MusicProviders.ToList().ForEach(mp => mp.OnMusicProviderChanged -= Mp_OnSelectionChanged);
        MusicProviders.Clear();

        Dictionary<int, MusicProviderBase> dic = musicProviders.ToDictionary(i => i.Id, i => i);

        IsLoading = true;
        
        try
        {
            var db = await dbCotextFactory.CreateDbContextAsync();

            db.MusicProviders.Include(p => p.Artists)
                .Select(mp => new MusicProviderViewModel(dic[mp.MusicProviderId], updateManager, toasts, dbCotextFactory)
                {
                    Name = mp.Name,
                    Image = mp.Image,
                    Uri = mp.Uri,
                    MusicProviderId = mp.MusicProviderId,
                    TrackedArtistsCount = mp.Artists.Count
                })
                .ToList()
                .ForEach(mp =>
                {
                    MusicProviders.Add(mp);                    
                    mp.OnMusicProviderChanged += Mp_OnSelectionChanged;
                });
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void Mp_OnSelectionChanged(MusicProviderViewModel mp)
    {
        MusicProviders.ToList().ForEach(mpe => mpe.IsActiveProvider = false);
        mp.IsActiveProvider = true;
        SelectedMusicProvider = mp;
        await SelectedMusicProvider.LoadAllArtists();
    }
}
