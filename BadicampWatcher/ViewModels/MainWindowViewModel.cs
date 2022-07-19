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

namespace MusicNewsWatcher.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IEnumerable<MusicProviderBase> musicProviders;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;
    private readonly MusicUpdateManager musicManager;

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

    public MainWindowViewModel()
    {
        LoadedCommand = new LambdaCommand(async e => await RefreshSource());       
        MusicProviders = new ObservableCollection<MusicProviderViewModel>();
    }

    public MainWindowViewModel(
        IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbCotextFactory,
        MusicUpdateManager musicManager) : this()
    {

        this.musicProviders = musicProviders;
        this.dbCotextFactory = dbCotextFactory;
        this.musicManager = musicManager;

        musicManager.OnUpdate += () => LoadedCommand.Execute(null);
    }


    private async Task RefreshSource()
    {
        Dictionary<int, MusicProviderBase> dic = musicProviders.ToDictionary(i => i.Id, i => i);

        IsLoading = true;
        await Task.Delay(TimeSpan.FromMilliseconds(250));
        
        try
        {
            var db = await dbCotextFactory.CreateDbContextAsync();

            db.MusicProviders.Include(p => p.Artists)
                .Select(mp => new MusicProviderViewModel(dic[mp.MusicProviderId], musicManager, dbCotextFactory)
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
                    mp.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName == nameof(mp.SelectedArtist))
                            RaisePropertyChanged(nameof(Title)); 
                    };
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
