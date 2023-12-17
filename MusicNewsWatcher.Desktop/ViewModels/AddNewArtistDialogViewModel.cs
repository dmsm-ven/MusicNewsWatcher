using MusicNewsWatcher.Desktop.Models.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class AddNewArtistDialogViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> contextFactory;

    public ObservableCollection<MusicProviderViewModel> MusicProviders { get; } = new ();

    MusicProviderViewModel? selectedMusicProvider;
    public MusicProviderViewModel? SelectedMusicProvider
    {
        get => selectedMusicProvider;
        set
        {
            if(selectedMusicProvider != null)
            {
                selectedMusicProvider.IsActiveProvider = false;
            }

            if(Set(ref selectedMusicProvider, value) && value != null)
            {
                selectedMusicProvider!.IsActiveProvider = true;
            }
        }
    }

    string artistSearchName;
    public string ArtistSearchName
    {
        get => artistSearchName;
        set
        {
            if(Set(ref artistSearchName, value))
            {
                ContextArtist.Name = artistSearchName;
                LoadSearchResultsCommand.Execute(null);

            }
        }
    }

    public bool IsEdit { get; private set; }

    public ArtistViewModel ContextArtist { get; private set; }

    List<ArtistViewModel> findedArtist;
    public List<ArtistViewModel> FindedArtists
    {
        get => findedArtist;
        private set => Set(ref findedArtist, value);
    }

    ArtistViewModel selectedFindedArtist;
    public ArtistViewModel SelectedFindedArtists
    {
        get => selectedFindedArtist;
        set
        {
            if(Set(ref selectedFindedArtist, value) && selectedFindedArtist != null)
            {
                ArtistSearchName = selectedFindedArtist.Name;
                ContextArtist.Name = selectedFindedArtist.Name;
                ContextArtist.Image = selectedFindedArtist.Image;
                ContextArtist.Uri = selectedFindedArtist.Uri;
            }
        }
    }

    public ICommand SubmitCommand { get; }
    public ICommand LoadSearchResultsCommand { get; }

    public AddNewArtistDialogViewModel(MusicProviderViewModel provider)
    {
        SubmitCommand = new LambdaCommand(Submit);
        LoadSearchResultsCommand = new LambdaCommand(async e => await LoadSearchResults());
        MusicProviders.Add(provider);
        SelectedMusicProvider = provider;
    }

    public AddNewArtistDialogViewModel(MusicProviderViewModel provider, 
        IDbContextFactory<MusicWatcherDbContext> contextFactory) : this(provider)
    {
        this.contextFactory = contextFactory;
        ContextArtist = new ArtistViewModel();
    }

    public AddNewArtistDialogViewModel(MusicProviderViewModel provider, 
        IDbContextFactory<MusicWatcherDbContext> contextFactory, 
        ArtistViewModel artist) : this(provider, contextFactory)
    {       
        ContextArtist = artist;
        IsEdit = true;
        artistSearchName = artist.Name;
    }

    private async Task LoadSearchResults()
    {
        if (SelectedMusicProvider == null || 
            string.IsNullOrWhiteSpace(artistSearchName) || 
            artistSearchName.Length < 3)
        {
            FindedArtists = Enumerable.Empty<ArtistViewModel>().ToList();
            return;
        }

        if(ArtistSearchName == SelectedFindedArtists?.Name)
        {
            return;
        }

        var searchResult = await SelectedMusicProvider
            .MusicProvider
            .SerchArtist(artistSearchName);

        var mappedArtists = searchResult
            .Select(i => new ArtistViewModel()
            {
                Name = i.Name,
                Image = i.Image,
                Uri = i.Uri,
            })
            .ToList();

        FindedArtists = mappedArtists;
    }
    
    private void Submit(object obj)
    {
        if(SelectedMusicProvider == null) { return; }

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
