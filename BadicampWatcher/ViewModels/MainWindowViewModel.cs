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
    private readonly BandcampWatcherDbContext _db;
    private readonly BandcampParser parser;
    private readonly ISettingsStorage settingsStorage;
    private readonly Timer autoUpdateTimer; 

    bool inProgress;
    public bool InProgress
    {
        get => inProgress;
        set
        {
            if (Set(ref inProgress, value))
            {
                settingsStorage.SetValue(nameof(SettingsRoot.LastCheck), (DateTime?)DateTime.Now);
                RaisePropertyChanged(nameof(Title));
            }
        }
    }

    public string Title
    {
        get => $"Bandcamp Watcher | Последнее обновление выполнено: {settingsStorage.Load().LastCheck}";
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
    public ICommand CheckUpdatesSelectedCommand { get; }
    public ICommand ShowSettingsWindowCommand { get; }
    public MainWindowViewModel(ISettingsStorage settingsStorage)
    {
        _db = new BandcampWatcherDbContext();
        parser = new BandcampParser(_db);
        this.settingsStorage = settingsStorage;

        AddArtistCommand = new LambdaCommand(AddTrackedArtist, e => true);
        DeleteArtistCommand = new LambdaCommand(DeleteArtists, e => SelectedArtist != null);
        EditArtistCommand = new LambdaCommand(EditArtist, e => SelectedArtist != null);
        CheckUpdatesAllCommand = new LambdaCommand(e => CheckUpdates(null), e => !InProgress);
        CheckUpdatesSelectedCommand = new LambdaCommand(e => CheckUpdates(SelectedArtist), e => !InProgress && SelectedArtist != null);
        ShowSettingsWindowCommand = new LambdaCommand(e => MessageBox.Show("NOT IMPLEMENTED!"), e => true);
        TrackedArtists = new ObservableCollection<ArtistModel>();

        LoadAllArtists();
        
        autoUpdateTimer = new Timer((int)TimeSpan.FromHours(1).TotalMilliseconds);
        autoUpdateTimer.Elapsed += AutoUpdateTimer_Elapsed;
        autoUpdateTimer.Start();

        if ((settingsStorage.Load().LastCheck ?? new DateTime()) + TimeSpan.FromHours(1) < DateTime.Now)
        {
            AutoUpdateTimer_Elapsed(null, null);
        }
        //_db.Albums.RemoveRange(_db.Albums);
        //_db.SaveChanges();
    }

    private void AutoUpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() => CheckUpdates(null));
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
                Albums = new ObservableCollection<AlbumModel>(artist.Albums.Select(a => new AlbumModel(_db)
                {
                    Image = a.Image,
                    Uri = a.Uri,
                    Created = a.Created,
                    IsViewed = a.IsViewed,
                    Title = a.Title
                }))
            };          
            TrackedArtists.Add(newArtist);

        }
    }

    private async void CheckUpdates(object obj)
    {
        InProgress = true;        
        try
        {

            var source = new List<ArtistModel>(obj == null ? TrackedArtists : new[] { SelectedArtist});

            await parser.CheckUpdates(source);

            SaveChangesIfNeed();
        }
        catch(Exception ex)
        {
            MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            InProgress = false;
        }
    }

    private void SaveChangesIfNeed()
    {
        bool hasChanges = false;
        foreach (var artist in TrackedArtists.Where(a => a.HasNew))
        {
            var dbArtist = _db.Artists.Single(a => a.Uri == artist.Uri);
            foreach (var album in artist.Albums)
            {
                if (dbArtist.Albums.FirstOrDefault(a => a.Uri == album.Uri) != null)
                {
                    continue;
                }
                dbArtist.Albums.Add(new Album() { Title = album.Title, Uri = album.Uri, Image = album.Image, Created = DateTime.Now });
            }
        }

        if (hasChanges)
        {
            _db.SaveChanges();
            SystemSounds.Exclamation.Play();
        }
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
