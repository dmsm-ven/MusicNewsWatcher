using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Services;
using BandcampWatcher.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly BandcampWatcherDbContext _db = new BandcampWatcherDbContext();
    private readonly BandcampParser parser = new BandcampParser();
    private readonly ISettingsStorage settingsStorage;
    private readonly Timer autoUpdateTimer; 


    bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set
        {
            if(Set(ref inProgress, value))
            {
                settingsStorage.SetValue(nameof(SettingsRoot.LastCheck), (DateTime?)DateTime.Now);
                RaisePropertyChanged(nameof(Title));
            }
            if(InProgress == false)
            {
                TrackedArtists.ToList().ForEach(a => a.CheckInProgress = false);
            }
        }
    }

    public string Title
    {
        get => $"Bandcamp Watcher | Последнее обновление выполнено: {settingsStorage.Load().LastCheck}";
    }

    public int progressCurrent = 0;
    public int ProgressCurrent
    {
        get => progressCurrent;
        set
        {
            Set(ref progressCurrent, value);
            TrackedArtists.ToList().ForEach(a => a.CheckInProgress = false);
            if (progressCurrent < TrackedArtists.Count)
            {
                TrackedArtists[progressCurrent].CheckInProgress = true;
            }
        }
    }

    ArtistModel selectedArtist;
    public ArtistModel SelectedArtist
    {
        get => selectedArtist;
        set => Set(ref selectedArtist, value);
    }
    public ObservableCollection<ArtistModel> TrackedArtists { get; }
    public ICommand AddArtistCommand { get; }
    public ICommand EditArtistCommand { get; }
    public ICommand DeleteArtistCommand { get; }
    public ICommand CheckUpdatesAllCommand { get; }
    public ICommand ShowSettingsWindowCommand { get; }
    public MainWindowViewModel(ISettingsStorage settingsStorage)
    {
        AddArtistCommand = new LambdaCommand(AddTrackedArtist, e => true);
        DeleteArtistCommand = new LambdaCommand(DeleteArtists, e => SelectedArtist != null);
        EditArtistCommand = new LambdaCommand(EditArtist, e => SelectedArtist != null);
        CheckUpdatesAllCommand = new LambdaCommand(CheckUpdatesAll, e => !InProgress);
        ShowSettingsWindowCommand = new LambdaCommand(e => MessageBox.Show("NOT IMPLEMENTED!"), e => true);
        TrackedArtists = new ObservableCollection<ArtistModel>();

        LoadAllArtists();
        this.settingsStorage = settingsStorage;
        autoUpdateTimer = new Timer((int)TimeSpan.FromHours(1).TotalMilliseconds);
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;
        autoUpdateTimer.Start();

        if ((settingsStorage.Load().LastCheck ?? new DateTime()) + TimeSpan.FromHours(1) < DateTime.Now)
        {
            AutoUpdateTimer_Elapsed(null, null);
        }
    }

    private void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() => CheckUpdatesAll(null));
    }

    private void LoadAllArtists()
    {
        foreach (var artist in _db.Artists.Include(a => a.Albums))
        {
            var newArtist = new ArtistModel()
            {
                Name = artist.Name,
                Image = artist.Image,
                Uri = artist.Uri,
                Albums = new ObservableCollection<AlbumModel>(artist.Albums.Select(a => new AlbumModel()
                {
                    Image = a.Image,
                    Uri = a.Uri,
                    Created = a.Created,
                    IsViewed = a.IsViewed,
                    Title = a.Title
                }))
            };
            newArtist.Albums.ToList().ForEach(a => a.OpenUrlClicked += () => Album_OpenUrlClicked(artist.ArtistId, a));
            TrackedArtists.Add(newArtist);

        }
    }

    private async void CheckUpdatesAll(object obj)
    {
        InProgress = true;
        try
        {
            var newAlbums = await parser.CheckUpdates(TrackedArtists, new Progress<int>(v => ProgressCurrent = v));
            foreach (var kvp in newAlbums)
            {
                var artist = _db.Artists.Single(a => a.Name == kvp.Key.Name);

                foreach (var album in kvp.Value)
                {
                    if (artist.Albums.FirstOrDefault(a => a.Uri == album.Uri) == null)
                    {
                        kvp.Key.Albums.Add(album);
                        album.OpenUrlClicked += () => Album_OpenUrlClicked(artist.ArtistId, album);
                        artist.Albums.Add(new Album() { Title = album.Title, Uri = album.Uri, Image = album.Image, Created = DateTime.Now });
                        kvp.Key.HasNew = true;
                    }
                }
            }
            _db.SaveChanges();
        }
        finally
        {
            InProgress = false;
        }
    }

    private void Album_OpenUrlClicked(int artist_id, AlbumModel albumModel)
    {
        var psi = new System.Diagnostics.ProcessStartInfo();
        psi.UseShellExecute = true;
        psi.FileName = albumModel.Uri;
        System.Diagnostics.Process.Start(psi);
        
        var albumEntity = _db.Albums.Single(a => a.ArtistId == artist_id && a.Title == albumModel.Title);
        albumModel.IsViewed = true;
        albumEntity.IsViewed = true;
        _db.SaveChanges();
    }

    private void AddTrackedArtist(object obj)
    {
        var dialogVm = new AddNewArtistDialogViewModel();
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {
            if(TrackedArtists.FirstOrDefault(a => a.Name == dialogVm.Name) != null) { return; }

            _db.Artists.Add(new Artist()
            {
                Name = dialogVm.Name,
                Image = dialogVm.Image,
                Uri = dialogVm.Uri,
            });
            _db.SaveChanges();

            TrackedArtists.Add(dialogVm.NewArtist);
        }
    }

    private void EditArtist(object obj)
    {
        var dialogVm = new AddNewArtistDialogViewModel(SelectedArtist);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {


            var changedArtist = _db.Artists.Single(a => a.Name == SelectedArtist.Name);

            SelectedArtist.Name = dialogVm.Name;
            SelectedArtist.Uri = dialogVm.Uri;
            SelectedArtist.Image = dialogVm.Image;

            changedArtist.Name = dialogVm.Name;
            changedArtist.Uri = dialogVm.Uri;
            changedArtist.Image = dialogVm.Image;

            _db.SaveChanges();
        }
    }

    private void DeleteArtists(object obj)
    {
        var dialogResult = MessageBox.Show($"Удалить '{SelectedArtist.Name}' ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (dialogResult != MessageBoxResult.Yes)
        {
            return;
        }

        _db.Artists.Remove(_db.Artists.Single(a => a.Name == SelectedArtist.Name));
        _db.SaveChanges();

        TrackedArtists.Remove(SelectedArtist);
        SelectedArtist = null;
    }
}
