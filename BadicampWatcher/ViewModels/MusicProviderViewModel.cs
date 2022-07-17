using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class MusicProviderViewModel : ViewModelBase
{
    public event Action OnClick;

    public int MusicProviderId { get; init; }
    public bool InProgress { get; private set; }

    public string Name { get; init; }
    public string Image { get; init; }
    public string Uri { get; init; }

    private readonly MusicProviderBase musicProvider;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbCotextFactory;

    ArtistViewModel? selectedArtist;
    public ArtistViewModel? SelectedArtist
    {
        get => selectedArtist;
        set => Set(ref selectedArtist, value);
    }

    public ObservableCollection<ArtistViewModel> TrackedArtists { get; }

    public ICommand OnClickCommand { get; }
    public ICommand AddArtistCommand { get; }
    public ICommand EditArtistCommand { get; }
    public ICommand DeleteArtistCommand { get; }
    public ICommand CheckUpdatesAllCommand { get; }
    public ICommand CheckUpdatesSelectedCommand { get; }

    public MusicProviderViewModel(MusicProviderBase musicProvider, IDbContextFactory<MusicWatcherDbContext> dbCotextFactory)
    {
        this.musicProvider = musicProvider;
        this.dbCotextFactory = dbCotextFactory;

        OnClickCommand = new LambdaCommand(e => OnClick?.Invoke());
        AddArtistCommand = new LambdaCommand(AddArtist);
        DeleteArtistCommand = new LambdaCommand(DeleteArtist, e => SelectedArtist != null);
        EditArtistCommand = new LambdaCommand(EditArtist, e => SelectedArtist != null);
        CheckUpdatesAllCommand = new LambdaCommand(e => CheckUpdates(null), e => !InProgress);
        CheckUpdatesSelectedCommand = new LambdaCommand(e => CheckUpdates(SelectedArtist), e => !InProgress && SelectedArtist != null);

        TrackedArtists = new ObservableCollection<ArtistViewModel>();
    }

    public void LoadAllArtists(object o)
    {
        TrackedArtists.Clear();

        using var db = dbCotextFactory.CreateDbContext();

            db.Artists
            .Where(a => a.MusicProviderId == MusicProviderId)
            .ToList()
            .Select(artist => new ArtistViewModel()
            {
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
            })
            .OrderByDescending(a => a.LastAlbumDate)
            .ToList()
            .ForEach(a => TrackedArtists.Add(a));
    }

    private void CheckUpdates(object? obj)
    {
        throw new NotImplementedException();
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
