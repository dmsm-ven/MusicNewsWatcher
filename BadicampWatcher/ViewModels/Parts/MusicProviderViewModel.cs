using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class MusicProviderViewModel : ViewModelBase
{
    public event Action<MusicProviderViewModel> OnMusicProviderChanged;

    public int MusicProviderId { get; init; }

    bool inProgress = false;
    public bool InProgress
    {
        get => inProgress;
        private set => Set(ref inProgress, value);
    }

    bool isActiveProvider;
    public bool IsActiveProvider
    {
        get => isActiveProvider;
        set => Set(ref isActiveProvider, value);
    }

    public string Name { get; init; }
    public string Image { get; init; }
    public string Uri { get; init; }

    private readonly MusicProviderBase musicProvider;
    private readonly MusicManager musicManager;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;

    ArtistViewModel? selectedArtist;
    public ArtistViewModel? SelectedArtist
    {
        get => selectedArtist;
        set => Set(ref selectedArtist, value);
    }

    public ObservableCollection<ArtistViewModel> TrackedArtists { get; } = new();

    public ICommand ChangeSelectedArtistCommand { get; }
    public ICommand CheckUpdatesAllCommand { get; }
    public ICommand CheckUpdatesSelectedCommand { get; }
    public ICommand AddArtistCommand { get; }
    public ICommand EditArtistCommand { get; }
    public ICommand DeleteArtistCommand { get; }

    public MusicProviderViewModel()
    {
        ChangeSelectedArtistCommand = new LambdaCommand(e => OnMusicProviderChanged?.Invoke(this));
        AddArtistCommand = new LambdaCommand(AddArtist);
        DeleteArtistCommand = new LambdaCommand(DeleteArtist, e => SelectedArtist != null);
        EditArtistCommand = new LambdaCommand(EditArtist, e => SelectedArtist != null);
        CheckUpdatesAllCommand = new LambdaCommand(e => CheckUpdates(null), e => !InProgress);
        CheckUpdatesSelectedCommand = new LambdaCommand(e => CheckUpdates(SelectedArtist), e => !InProgress && SelectedArtist != null);
    }

    public MusicProviderViewModel(
        MusicProviderBase musicProvider, 
        MusicManager musicManager,
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory) : this()
    {
        this.musicProvider = musicProvider;
        this.musicManager = musicManager;
        this.dbCotextFactory = dbContextFactory;

    }

    public async Task LoadAllArtists(MusicWatcherDbContext db)
    {
        TrackedArtists.Clear();

        var artists = await db.Artists
            .Where(a => a.MusicProviderId == MusicProviderId)
            .ToListAsync();

        var artistsVm = artists.Select(artist => new ArtistViewModel(dbCotextFactory)
        {
            ArtistId = artist.ArtistId,
            Name = artist.Name,
            Image = artist.Image,
            Uri = artist.Uri,
            Albums = new ObservableCollection<AlbumViewModel>(artist.Albums.Select(a => new AlbumViewModel
            {
                Image = a.Image,
                Uri = a.Uri,
                Created = a.Created,
                IsViewed = a.IsViewed,
                Title = a.Title
            }))
        });

        foreach(var artist in artistsVm.OrderByDescending(a => a.LastAlbumDate))
        {
            TrackedArtists.Add(artist);
            artist.OnArtistChanged += (e) =>
            {
                TrackedArtists.ToList().ForEach(ta => ta.IsActiveArtist = false);
                e.IsActiveArtist = true;
                SelectedArtist = e;
            };
        }
    }

    private async void CheckUpdates(object? obj)
    {
        InProgress = true;
        try
        {
            var artist = (obj as ArtistViewModel);
            if (artist != null)
            {
                await musicManager.CheckUpdatesForArtist(this.musicProvider, artist.ArtistId);
            }
            else
            {
                await musicManager.CheckUpdatesForProvider(musicProvider);
            }
        }
        finally
        {
            InProgress = false;
        }
    }

    private void AddArtist(object obj)
    {
        var dialogVm = new AddNewArtistDialogViewModel(dbCotextFactory);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {
            TrackedArtists.Add(dialogVm.NewArtist);
        }
    }

    private void EditArtist(object obj)
    {
        var dialogVm = new AddNewArtistDialogViewModel(dbCotextFactory, SelectedArtist!);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        dialogWindow.ShowDialog();
    }

    private void DeleteArtist(object obj)
    {
        var dialogResult = MessageBox.Show($"Удалить '{SelectedArtist!.Name}' ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (dialogResult == MessageBoxResult.Yes)
        {
            using (var db = dbCotextFactory.CreateDbContext())
            {
                db.Artists.Remove(db.Artists.Find(SelectedArtist.ArtistId)!);
                db.SaveChanges();
            }

            TrackedArtists.Remove(SelectedArtist);
            SelectedArtist = null;
        }
    }

}
