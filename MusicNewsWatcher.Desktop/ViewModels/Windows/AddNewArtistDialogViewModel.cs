using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class AddNewArtistDialogViewModel : ObservableObject
{
    private readonly MusicWatcherDbContext dbContext;

    public bool IsEdit { get; private set; }

    [ObservableProperty]
    private ObservableCollection<MusicProviderViewModel> musicProviders = new();

    [ObservableProperty]
    private MusicProviderViewModel? selectedMusicProvider;

    [ObservableProperty]
    private string artistSearchName;

    public ArtistViewModel ContextArtist { get; init; }

    [ObservableProperty]
    private List<ArtistViewModel> findedArtist = new();

    [ObservableProperty]
    private ArtistViewModel selectedFindedArtist;

    public AddNewArtistDialogViewModel(MusicProviderViewModel provider, ArtistViewModel? artist)
    {
        throw new NotImplementedException();
        /*IsEdit = artist != null;
        ContextArtist = artist ?? new ArtistViewModel(provider);
        artistSearchName = artist.Name;

        MusicProviders.Add(provider);
        SelectedMusicProvider = provider;
        dbContext = App.HostContainer.Services.GetRequiredService<MusicWatcherDbContext>();
        */
    }

    partial void OnSelectedFindedArtistChanged(ArtistViewModel? oldValue, ArtistViewModel newValue)
    {
        ArtistSearchName = newValue.Name;
        ContextArtist.Name = newValue.Name;
        ContextArtist.Image = newValue.Image;
        ContextArtist.Uri = newValue.Uri;
    }

    async partial void OnArtistSearchNameChanged(string oldName, string newName)
    {
        ContextArtist.Name = newName;
        await LoadSearchResults();
    }

    [RelayCommand]
    private async Task LoadSearchResults()
    {
        throw new NotImplementedException();
        /*if (SelectedMusicProvider == null ||
            string.IsNullOrWhiteSpace(artistSearchName) ||
            artistSearchName.Length < 3)
        {
            FindedArtist.Clear();
            return;
        }

        if (ArtistSearchName == SelectedFindedArtist?.Name)
        {
            return;
        }

        var searchResult = await SelectedMusicProvider
            .Template
            .SerchArtist(artistSearchName);

        var mappedArtists = searchResult
            .Select(i => new ArtistViewModel(this.SelectedMusicProvider)
            {
                Name = i.Name,
                Image = i.Image,
                Uri = i.Uri,
            })
            .ToList();

        FindedArtist = mappedArtists;
        */
    }

    [RelayCommand]
    private void Submit(object obj)
    {
        if (SelectedMusicProvider == null) { return; }

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
                dbContext.Artists.Remove(dbContext.Artists.Find(ContextArtist.ArtistId));
                dbContext.Artists.Add(entity);
                dbContext.SaveChanges();
            }
        }
        else // add
        {
            dbContext.Artists.Add(entity);
            dbContext.SaveChanges();
        }
        (obj as Window).DialogResult = true;
    }
}
