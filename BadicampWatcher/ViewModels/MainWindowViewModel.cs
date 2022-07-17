using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Services;
using BandcampWatcher.Views;
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

namespace BandcampWatcher.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IEnumerable<MusicProviderBase> musicProviders;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;
    private readonly MusicManager musicManager;

    MusicProviderViewModel selectedMusicProvider;
    public MusicProviderViewModel SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set => Set(ref selectedMusicProvider, value);
    }

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; }    
    public ICommand LoadedCommand { get; }

    public MainWindowViewModel()
    {
        LoadedCommand = new LambdaCommand(async e => await RefreshSource());       
        MusicProviders = new ObservableCollection<MusicProviderViewModel>();
        AddDesignTimeProviders();
    }

    private void AddDesignTimeProviders()
    {
        MusicProviders.Add(new MusicProviderViewModel(new MusifyMusicProvider(), null, null)
        {
            Name = "Musify"
        });
        MusicProviders.Add(new MusicProviderViewModel(new BandcampMusicProvider(), null, null)
        {
            Name = "Bandcamp"
        });
    }

    public MainWindowViewModel(
        IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbCotextFactory,
        MusicManager musicManager) : this()
    {

        this.musicProviders = musicProviders;
        this.dbCotextFactory = dbCotextFactory;
        this.musicManager = musicManager;

        musicManager.OnUpdate += () => LoadedCommand.Execute(null);
    }


    private async Task RefreshSource()
    {
        MusicProviders.Clear();

        Dictionary<int, MusicProviderBase> dic = musicProviders.ToDictionary(i => i.Id, i => i);
        var db = await dbCotextFactory.CreateDbContextAsync();

        db.MusicProviders.Include(p => p.Artists)
            .Select(mp => new MusicProviderViewModel(dic[mp.MusicProviderId], musicManager, dbCotextFactory)
            {
                Name = mp.Name,
                Image = mp.Image,
                Uri = mp.Uri,
                MusicProviderId = mp.MusicProviderId
            })
            .ToList()
            .ForEach(async mp => 
            { 
                MusicProviders.Add(mp);
                await mp.LoadAllArtists(db);
                mp.OnMusicProviderChanged += Mp_OnSelectionChanged;
            });

        SelectedMusicProvider = SelectedMusicProvider ?? MusicProviders.FirstOrDefault();
    }

    private void Mp_OnSelectionChanged(MusicProviderViewModel mp)
    {
        MusicProviders.ToList().ForEach(mpe => mpe.IsActiveProvider = false);
        mp.IsActiveProvider = true;
        SelectedMusicProvider = mp;
    }
}
