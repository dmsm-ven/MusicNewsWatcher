using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using MusicNewsWatcher.Services;
using MusicNewsWatcher.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Messages;

namespace MusicNewsWatcher.ViewModels;

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
            if(SelectedMusicProvider != null)
            {
                title += $" | {SelectedMusicProvider.Name}";

                if(SelectedMusicProvider.SelectedArtist != null)
                {
                    title += $" | исполнитель {SelectedMusicProvider.SelectedArtist.DisplayName}";
                }
            }

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
        SettingsCommand = new LambdaCommand(e => { });       
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
