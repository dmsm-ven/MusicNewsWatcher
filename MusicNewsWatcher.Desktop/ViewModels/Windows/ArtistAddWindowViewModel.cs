using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicNewsWatcher.ApiClient;
using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.ViewModels.Items;
using System.Collections.ObjectModel;
using System.Windows;

namespace MusicNewsWatcher.Desktop.ViewModels.Windows;

public partial class ArtistAddWindowViewModel : ObservableObject
{
    public bool IsEdit { get; init; }

    [ObservableProperty]
    private ObservableCollection<MusicProviderViewModel> musicProviders = new();

    [ObservableProperty]
    private MusicProviderViewModel? selectedMusicProvider = null;

    // Все поля формы живут в самом окне. Исполнитель в списке (ContextArtist) меняется только после успешного сохранения,
    // поэтому "Отмена" ничего не портит.
    [ObservableProperty]
    private string artistSearchName = string.Empty;

    [ObservableProperty]
    private string artistUri = string.Empty;

    [ObservableProperty]
    private string artistImage = string.Empty;

    public ArtistViewModel ContextArtist { get; init; }

    [ObservableProperty]
    private ObservableCollection<ArtistViewModel> findedArtist = new();

    [ObservableProperty]
    private ArtistViewModel? selectedFindedArtist = null;

    private readonly MusicNewsWatcherApiClient apiClient;

    public ArtistAddWindowViewModel(MusicProviderViewModel provider, MusicNewsWatcherApiClient apiClient, ArtistViewModel contextArtist)
    {
        this.apiClient = apiClient;
        this.ContextArtist = contextArtist;
        this.IsEdit = !string.IsNullOrWhiteSpace(ContextArtist.Uri);

        MusicProviders.Add(provider);
        SelectedMusicProvider = provider;

        if (IsEdit)
        {
            ArtistSearchName = ContextArtist.Name;
            ArtistUri = ContextArtist.Uri;
            ArtistImage = ContextArtist.Image;
        }
    }

    partial void OnSelectedFindedArtistChanged(ArtistViewModel? oldValue, ArtistViewModel? newValue)
    {
        ArtistSearchName = newValue?.Name ?? string.Empty;
        ArtistImage = newValue?.Image ?? string.Empty;
        ArtistUri = newValue?.Uri ?? string.Empty;
    }

    [RelayCommand]
    private Task LoadSearchResults()
    {
        // Поиск по сайтам временно отключен
        return Task.CompletedTask;
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
    private async Task Submit(object? obj)
    {
        if (SelectedMusicProvider == null || obj is not Window window)
        {
            return;
        }

        string name = ArtistSearchName.Trim();
        string uri = ArtistUri.Trim();
        string image = ArtistImage.Trim();

        if (name.Length == 0)
        {
            MessageBox.Show("Укажите имя исполнителя", "Проверьте данные", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (IsEdit)
            {
                var dto = new ArtistDto()
                {
                    MusicProviderId = SelectedMusicProvider.MusicProviderId,
                    ArtistId = ContextArtist.ArtistId,
                    Name = name,
                    Uri = uri,
                    Image = image
                };
                await apiClient.UpdateArtist(dto);

                ContextArtist.Name = name;
                ContextArtist.Uri = uri;
                ContextArtist.Image = image;
            }
            else
            {
                var artist = new CreateArtistDto(SelectedMusicProvider.MusicProviderId, name, uri, image);
                await apiClient.CreateArtist(artist);
            }

            window.DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось сохранить исполнителя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
