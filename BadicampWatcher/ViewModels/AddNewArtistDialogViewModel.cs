using MusicNewsWatcher.DataAccess;
using MusicNewsWatcher.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MusicNewsWatcher.ViewModels;

public class AddNewArtistDialogViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> contextFactory;

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; } = new ();

    MusicProviderViewModel? selectedMusicProvider;
    public MusicProviderViewModel? SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set => Set(ref selectedMusicProvider, value);
    }

    public bool IsEdit { get; private set; }

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
        ContextArtist = new ArtistViewModel(contextFactory, null, null, null);
    }

    public AddNewArtistDialogViewModel(IDbContextFactory<MusicWatcherDbContext> contextFactory, ArtistViewModel artist) : this(contextFactory)
    {       
        ContextArtist = artist;
        IsEdit = true;
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

        SelectedMusicProvider = MusicProviders.FirstOrDefault(mp => mp.MusicProviderId == (ContextArtist?.MusicProviderId ?? 0));
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
                MusicProviderId = SelectedMusicProvider.MusicProviderId,
                ArtistId = ContextArtist.ArtistId
            };

            if (IsEdit)
            {
                if (ContextArtist.ArtistId != 0)
                {
                    db.Artists.Remove(db.Artists.Find(ContextArtist.ArtistId));
                    db.Artists.Add(entity);
                    db.SaveChanges();
                }
            }
            else // add
            {
                db.Artists.Add(entity);
                db.SaveChanges();
            }         
            
        }

        (obj as Window).DialogResult = true;
    }
}
