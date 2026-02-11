using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using MusicNewsWatcher.Desktop.ViewModels.Mappers;
using System.Collections.ObjectModel;
using System.Windows;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class ArtistAddWindowViewModel : ObservableObject
{
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
    private readonly MusicNewsWatcherApiClient apiClient;

    public ArtistAddWindowViewModel(MusicProviderViewModel provider, MusicNewsWatcherApiClient apiClient)
    {
        ContextArtist = App.HostContainer.Services.GetRequiredService<ViewModelFactory<ArtistViewModel>>().Create();
        ContextArtist.Initialize(provider, new(provider.MusicProviderId, 0, "Новый исполнитель", provider.Uri, string.Empty));

        MusicProviders.Add(provider);
        SelectedMusicProvider = provider;
        this.apiClient = apiClient;
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
        //throw new NotImplementedException();
        /*
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
        */
    }

    [RelayCommand]
    private async Task Submit(object obj)
    {
        if (SelectedMusicProvider == null)
        {
            return;
        }

        if (IsEdit)
        {
            var dto = this.ContextArtist.ToDto();
            await apiClient.UpdateArtist(dto);
        }
        else
        {
            var artist = new CreateArtistDto(SelectedMusicProvider.MusicProviderId, ContextArtist.Name, ContextArtist.Uri, ContextArtist.Image);
            await apiClient.CreateArtist(artist);
        }

        (obj as Window).DialogResult = true;
    }
}
