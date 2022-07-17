using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class AddNewArtistDialogViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> contextFactory;

    public ArtistViewModel NewArtist { get; private set; }

    public string SubmitCommandTitle { get; private set; }

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; } = new ();

    MusicProviderViewModel selectedMusicProvider;
    public MusicProviderViewModel SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set => Set(ref selectedMusicProvider, value);
    }

    public ArtistViewModel ContextArtist { get; private set; }

    public ICommand SubmitCommand { get; }
    public ICommand LoadedCommand { get; }

    public AddNewArtistDialogViewModel()
    {
        LoadedCommand = new LambdaCommand(Loaded);
        SubmitCommand = new LambdaCommand(Submit);
        
    }

    public AddNewArtistDialogViewModel(IDbContextFactory<MusicWatcherDbContext> contextFactory) : this()
    {
        this.contextFactory = contextFactory;
        SubmitCommandTitle = "Добавить";
        ContextArtist = new ArtistViewModel(contextFactory);
    }

    public AddNewArtistDialogViewModel(IDbContextFactory<MusicWatcherDbContext> contextFactory, ArtistViewModel artist) : this(contextFactory)
    {
        SubmitCommandTitle = "Изменить";
        ContextArtist = artist;
    }
    private void Loaded(object obj)
    {
        using var db = contextFactory.CreateDbContext();

        db.MusicProviders
            .Select(mp => new MusicProviderViewModel()
            {
                MusicProviderId = mp.MusicProviderId,
                Name = mp.Name,
                Uri = mp.Uri
            })
            .ToList()
            .ForEach(item => MusicProviders.Add(item));
    }

    private void Submit(object obj)
    {
        using (var db = contextFactory.CreateDbContext())
        {
            var entity = new ArtistEntity()
            {
                Name = ContextArtist.Name,
                Image = ContextArtist.Image,
                Uri = ContextArtist.Uri,
                MusicProviderId = SelectedMusicProvider.MusicProviderId
            };

            db.Artists.Add(entity);
            db.SaveChanges();

            NewArtist = new ArtistViewModel(contextFactory)
            {
                Name = ContextArtist.Name,
                Uri = ContextArtist.Uri,
                Image = ContextArtist.Image,
                ArtistId = entity.ArtistId
            };
        }

        (obj as Window).DialogResult = true;
    }
}
