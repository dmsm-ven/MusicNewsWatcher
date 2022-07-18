using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using MusicNewsWatcher.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicNewsWatcher.ViewModels;

public class MusicProviderViewModel : ViewModelBase
{
    private readonly MusicProviderBase musicProvider;
    private readonly MusicManager musicManager;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;

    public event Action<MusicProviderViewModel> OnMusicProviderChanged;
    public ObservableCollection<ArtistViewModel> TrackedArtists { get; } = new();

    public int MusicProviderId { get; init; }
    public string Name { get; init; }
    public string Image { get; init; }
    public string Uri { get; init; }

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

    ArtistViewModel? selectedArtist;
    public ArtistViewModel? SelectedArtist
    {
        get => selectedArtist;
        set => Set(ref selectedArtist, value);
    }

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
        if(TrackedArtists.Count > 0) { return; }

        TrackedArtists.Clear();

        var artists = await db.Artists
            .Where(a => a.MusicProviderId == MusicProviderId)
            .ToListAsync();

        var artistsVm = artists.Select(artist => new ArtistViewModel(dbCotextFactory, musicManager, musicProvider)
        {
            ArtistId = artist.ArtistId,
            MusicProviderId = artist.MusicProviderId,
            Name = artist.Name,
            Image = artist.Image,
            Uri = artist.Uri,
            Albums = new ObservableCollection<AlbumViewModel>(artist.Albums
                .Select(a => new AlbumViewModel(dbCotextFactory, musicManager, musicProvider)
            {
                Image = a.Image,
                Uri = a.Uri,
                Created = a.Created,
                IsViewed = a.IsViewed,
                Title = a.Title,
                AlbumId = a.AlbumId,
            }))
        })
        .OrderByDescending(a => a.LastAlbumDate);

        foreach(var artist in artistsVm)
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
                await musicManager.CheckUpdatesForArtistAsync(this.musicProvider, artist.ArtistId);
            }
            else
            {
                await musicManager.CheckUpdatesForProviderAsync(musicProvider);
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
