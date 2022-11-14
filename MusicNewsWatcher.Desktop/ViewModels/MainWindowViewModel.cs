using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicNewsWatcher.Desktop.Services;
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
    private readonly IToastsNotifier toasts;
    
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
            if (IsLoading) { return; }

            if(selectedMusicProvider != null)
            {
                selectedMusicProvider.IsActiveProvider = false;
            }

            if (Set(ref selectedMusicProvider, value) && value != null)
            {
                selectedMusicProvider!.IsActiveProvider = true;
            }
        }
    }

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; } 
    
    public ICommand LoadedCommand { get; }
    public ICommand CheckUpdatesAllCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand OpenDownloadsFolderCommand { get; }
    public ICommand AddArtistCommand { get; }

    public MainWindowViewModel()
    {
        AddArtistCommand = new LambdaCommand(ShowNewArtistWindow);
        CheckUpdatesAllCommand = new LambdaCommand(async e => await LoadItemsSource(), e => !IsLoading);
        OpenDownloadsFolderCommand = new LambdaCommand(e => FileBrowserHelper.OpenDownloadsFolder());       
        SettingsCommand = new LambdaCommand(ShowSettingsWindow);       
        LoadedCommand = new LambdaCommand(async e => await LoadItemsSource());       
        MusicProviders = new ObservableCollection<MusicProviderViewModel>();
    }

    public MainWindowViewModel(
        IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbCotextFactory,
        MusicUpdateManager updateManager,
        IToastsNotifier toasts) : this()
    {

        this.musicProviders = musicProviders;
        this.dbCotextFactory = dbCotextFactory;
        this.toasts = toasts;
        updateManager.OnNewAlbumsFound += UpdateManager_OnNewAlbumsFound;
    }

    private void UpdateManager_OnNewAlbumsFound(object? sender, NewAlbumsFoundEventArgs e)
    {
        string toastsMessage = string.Join("\r\n", e.NewAlbums.Select((album, i) => $"{i + 1}) {album?.name ?? "<Без названия>"}"));
        toasts.ShowInformation("Найдены новые альбомы:\r\n" + toastsMessage);
    }

    private void ShowNewArtistWindow(object obj)
    {
        var dialogWindow = App.HostContainer.Services.GetRequiredService<AddNewArtistDialog>();
        dialogWindow.DataContext = new AddNewArtistDialogViewModel(SelectedMusicProvider, dbCotextFactory);
        dialogWindow.ShowDialog();
    }

    private void ShowSettingsWindow(object obj)
    {
        var window = App.HostContainer.Services.GetRequiredService<SettingsWindow>();
        window.DataContext = App.HostContainer.Services.GetRequiredService<SettingsWindowViewModel>();
        window.ShowDialog();
    }

    private async Task LoadItemsSource()
    {
        SelectedMusicProvider = null;
        MusicProviders.Clear();

        Dictionary<int, MusicProviderBase> dic = musicProviders.ToDictionary(i => i.Id, i => i);
        IsLoading = true;
       
        using var db = await dbCotextFactory.CreateDbContextAsync();

        var source = await db.MusicProviders
            .Include(p => p.Artists)
            .ToListAsync();

        foreach(var provider in source)
        {           
            var providerVm = new MusicProviderViewModel()
            {
                MusicProvider = dic[provider.MusicProviderId],
                Name = provider.Name,
                Image = provider.Image,
                Uri = provider.Uri,
                MusicProviderId = provider.MusicProviderId,
                TrackedArtistsCount = provider.Artists.Count,
            };
            
            foreach (var artist in provider.Artists)
            {
                var artistVm = new ArtistViewModel(providerVm)
                {
                    Name = artist.Name,
                    Uri = artist.Uri,
                    Image = artist.Image,
                    ArtistId = artist.ArtistId
                };

                artistVm.OnArtistChanged += (e) => providerVm.SelectedArtist = e;
                providerVm.TrackedArtists.Add(artistVm);
            }

            providerVm.OnMusicProviderChanged += (e) => SelectedMusicProvider = e;
            MusicProviders.Add(providerVm);
        }

        IsLoading = false;
        SelectedMusicProvider = MusicProviders.LastOrDefault();


        
    }

}
