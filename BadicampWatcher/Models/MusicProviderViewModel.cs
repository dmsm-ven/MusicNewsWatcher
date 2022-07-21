﻿using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using MusicNewsWatcher.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Messages;

namespace MusicNewsWatcher.ViewModels;

public class MusicProviderViewModel : ViewModelBase
{
    private readonly MusicProviderBase musicProvider;
    private readonly MusicUpdateManager musicManager;
    private readonly Notifier toasts;
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContextFactory;

    public event Action<MusicProviderViewModel> OnMusicProviderChanged;
    public ObservableCollection<ArtistViewModel> TrackedArtists { get; } = new();

    public int MusicProviderId { get; init; }
    public string Name { get; init; }
    public string Image { get; init; }
    public string Uri { get; init; }

    int trackedArtistsCount;
    public int TrackedArtistsCount
    {
        get => TrackedArtists.Count > 0 ? TrackedArtists.Count : trackedArtistsCount;
        set => Set(ref trackedArtistsCount, value);
    }

    bool inProgress = false;
    public bool InProgress
    {
        get => inProgress;
        set => Set(ref inProgress, value);
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
    public ICommand CheckUpdatesSelectedCommand { get; }
    public ICommand AddArtistCommand { get; }
    public ICommand EditArtistCommand { get; }
    public ICommand DeleteArtistCommand { get; }
    

    public MusicProviderViewModel()
    {
        ChangeSelectedArtistCommand = new LambdaCommand(e => OnMusicProviderChanged?.Invoke(this));
        AddArtistCommand = new LambdaCommand(async e => await AddArtist());
        DeleteArtistCommand = new LambdaCommand(DeleteArtist, e => SelectedArtist != null);
        EditArtistCommand = new LambdaCommand(EditArtist, e => SelectedArtist != null);
        TrackedArtists.CollectionChanged += (o, e) => RaisePropertyChanged(nameof(TrackedArtistsCount));
    }

    public MusicProviderViewModel(
        MusicProviderBase musicProvider, 
        MusicUpdateManager musicManager,
        Notifier toasts,
        IDbContextFactory<MusicWatcherDbContext> dbContextFactory) : this()
    {
        this.musicProvider = musicProvider;
        this.musicManager = musicManager;
        this.toasts = toasts;
        this.dbContextFactory = dbContextFactory;
    }

    public async Task LoadAllArtists(bool forced = false)
    {
        if (TrackedArtists.Count > 0 && !forced) { return; }
     
        InProgress = true;


        using var db = await dbContextFactory.CreateDbContextAsync();
        
        var dbArtists = await db.Artists
            .Where(a => a.MusicProviderId == MusicProviderId)
            .ToListAsync();

        if (TrackedArtists.Count != dbArtists.Count || forced)
        {
            var artistsVm = dbArtists
            .Select(artist => new ArtistViewModel(dbContextFactory, musicManager, musicProvider, toasts)
            {
                ArtistId = artist.ArtistId,
                MusicProviderId = artist.MusicProviderId,
                Name = artist.Name,
                Image = artist.Image,
                Uri = artist.Uri
            })
            .OrderByDescending(a => a.LastAlbumDate);

            TrackedArtists.ToList().ForEach(artist => artist.OnArtistChanged -= Artist_OnArtistChanged);
            TrackedArtists.Clear();

            foreach (var artist in artistsVm)
            {
                TrackedArtists.Add(artist);
                artist.OnArtistChanged += Artist_OnArtistChanged;
            }
        }
        
        InProgress = false;
    }

    private async Task AddArtist()
    {
        var dialogVm = new AddNewArtistDialogViewModel(dbContextFactory);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {
            await LoadAllArtists(forced: true);
            Artist_OnArtistChanged(TrackedArtists.LastOrDefault()!);
        }
    }

    private async void EditArtist(object obj)
    {
        var dialogVm = new AddNewArtistDialogViewModel(dbContextFactory, SelectedArtist!);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {
            await LoadAllArtists(forced: true);
            Artist_OnArtistChanged(SelectedArtist);
            toasts.ShowSuccess($"Данные обновлены");
        }
    }

    private void DeleteArtist(object obj)
    {
        var dialogResult = MessageBox.Show($"Удалить '{SelectedArtist!.Name}' ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (dialogResult == MessageBoxResult.Yes)
        {
            using (var db = dbContextFactory.CreateDbContext())
            {
                db.Artists.Remove(db.Artists.Find(SelectedArtist.ArtistId)!);
                db.SaveChanges();
            }
            toasts.ShowSuccess($"Исполнитель удален из списка на отслеживание");
            TrackedArtists.Remove(SelectedArtist);
            SelectedArtist = null;
        }
    }

    private void Artist_OnArtistChanged(ArtistViewModel? e)
    {
        if(SelectedArtist != null)
        {
            SelectedArtist.IsActiveArtist = false;           
        }
     
        SelectedArtist = e;
        GC.Collect(2);
    }
}
