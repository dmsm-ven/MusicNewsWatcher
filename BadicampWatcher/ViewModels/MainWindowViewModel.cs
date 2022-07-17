using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Services;
using BandcampWatcher.Views;
using Microsoft.EntityFrameworkCore;
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

    public string Title
    {
        get => "Music News Watcher";
    }

    MusicProviderViewModel selectedMusicProvider;
    public MusicProviderViewModel SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set 
        { 
            if(Set(ref selectedMusicProvider, value))
            {
                selectedMusicProvider.LoadAllArtists(null);
            }
        }
    }

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; }    
    public ICommand LoadedCommand { get; }

    public MainWindowViewModel()
    {
        LoadedCommand = new LambdaCommand(Loaded);
        MusicProviders = new ObservableCollection<MusicProviderViewModel>()
        {
            new MusicProviderViewModel(new MusifyMusicProvider(), null)
            {
                Name = "Musify"
            },
            new MusicProviderViewModel(new BandcampMusicProvider(), null)
            {
                Name = "Bandcamp"
            },
        };
    }

    public MainWindowViewModel(
        IEnumerable<MusicProviderBase> musicProviders, 
        IDbContextFactory<MusicWatcherDbContext> dbCotextFactory) : this()
    {

        this.musicProviders = musicProviders;
        this.dbCotextFactory = dbCotextFactory;
    }

    private void Loaded(object obj)
    {
        MusicProviders.Clear();

        Dictionary<int, MusicProviderBase> dic = musicProviders.ToDictionary(i => i.Id, i => i);

        dbCotextFactory.CreateDbContext().MusicProviders
            .Select(mp => new MusicProviderViewModel(dic[mp.MusicProviderId], dbCotextFactory)
            {
                Name = mp.Name,
                Image = mp.Image,
                Uri = mp.Uri,
                MusicProviderId = mp.MusicProviderId
            })
            .ToList()
            .ForEach(mp => 
            { 
                MusicProviders.Add(mp);
                mp.OnClick += () => SelectedMusicProvider = mp;
            });
    }

}
