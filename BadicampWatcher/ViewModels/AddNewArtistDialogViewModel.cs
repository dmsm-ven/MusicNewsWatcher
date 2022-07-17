using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class AddNewArtistDialogViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContext;

    public ArtistViewModel NewArtist { get; private set; }

    public string SubmitCommandTitle { get; private set; }

    public IReadOnlyList<MusicProviderModel> MusicProviders
    {
        get => dbContext
            .CreateDbContext()
            .MusicProviders
            .Select(mp => new MusicProviderModel()
            {
                MusicProviderId = mp.MusicProviderId,
                Name = mp.Name,
                Uri = mp.Uri
            })
            .ToList();
    }

    MusicProviderModel selectedMusicProvider;
    public MusicProviderModel SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set => Set(ref selectedMusicProvider, value);
    }

    string name;
    public string Name 
    { 
        get => name; 
        set => Set(ref name, value); 
    }

    string image;
    public string Image 
    { 
        get => image;
        set => Set(ref image, value);
    }

    string uri;
    public string Uri { get => uri; set => Set(ref uri, value); }

    public ICommand SubmitCommand { get; }

    public AddNewArtistDialogViewModel(IDbContextFactory<MusicWatcherDbContext> contextFactory, ArtistViewModel artist) : this(contextFactory)
    {
        Name = artist.Name;
        Uri = artist.Uri;
        Image = artist.Image;
        SubmitCommandTitle = "Изменить";
    }

    public AddNewArtistDialogViewModel(IDbContextFactory<MusicWatcherDbContext> contextFactory)
    {
        SubmitCommand = new LambdaCommand(Submit, e =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Image) &&
            !string.IsNullOrWhiteSpace(Uri));
        SubmitCommandTitle = "Добавить";
        this.dbContext = contextFactory;
    }

    private void Submit(object obj)
    {


        using (var db = dbContext.CreateDbContext())
        {
            var entity = new ArtistEntity()
            {
                Name = Name,
                Image = Image,
                Uri = Uri,
                MusicProviderId = SelectedMusicProvider.MusicProviderId
            };

            db.Artists.Add(entity);
            db.SaveChanges();

            NewArtist = new ArtistViewModel()
            {
                Name = Name,
                Uri = Uri,
                Image = Image,
                ArtistId = entity.ArtistId
            };
        }

        (obj as Window).DialogResult = true;
    }
}
