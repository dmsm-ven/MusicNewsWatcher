using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class AddOrEditArtistDialogViewModel : ObservableObject
{
    private readonly MusicWatcherDbContext dbContext;

    public bool IsEdit { get; init; }

    [ObservableProperty]
    private ObservableCollection<MusicProviderViewModel> musicProviders = new();

    [ObservableProperty]
    private MusicProviderViewModel? selectedMusicProvider;

    [ObservableProperty]
    private string artistSearchName;

    public ArtistViewModel ContextArtist { get; init; }

    [ObservableProperty]
    private ObservableCollection<ArtistViewModel> findedArtist = new();

    [ObservableProperty]
    private ArtistViewModel selectedFindedArtist;

    public AddOrEditArtistDialogViewModel(MusicProviderViewModel provider)
    {
        dbContext = App.HostContainer.Services.GetRequiredService<MusicWatcherDbContext>();
        ContextArtist = App.HostContainer.Services.GetRequiredService<ViewModelFactory<ArtistViewModel>>().Create();
        ContextArtist.Initialize(provider, 0, name: "", image: "", uri: "");

        MusicProviders.Add(provider);
        SelectedMusicProvider = provider;
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
        FindedArtist.Clear();

        if (SelectedMusicProvider == null ||
            string.IsNullOrWhiteSpace(ArtistSearchName) ||
            ArtistSearchName.Length < 3)
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
            .SerchArtist(ArtistSearchName);

        var mappedArtists = searchResult
            .Select(i => ArtistViewModel.Create(ContextArtist, i.Name, i.Image, i.Uri))
            .ToList();

        FindedArtist.AddRange(mappedArtists);
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
