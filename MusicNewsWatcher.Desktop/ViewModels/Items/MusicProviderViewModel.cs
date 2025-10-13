using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicNewsWatcher.Core.Extensions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

public partial class MusicProviderViewModel : ObservableObject
{
    public MusicProviderBase Template { get; private set; }
    public string Name => Template.Name;
    public int MusicProviderId => Template.Id;
    public string Image { get; private set; }
    public string Uri { get; private set; }

    private readonly MusicWatcherDbContext dbContext;
    private readonly IDialogWindowService dialogWindowService;
    private readonly IToastsNotifier toasts;
    private readonly ViewModelFactory<ArtistViewModel> artistVmFactory;

    private bool isLoaded = false;
    private bool isInitialized = false;
    private int initialTrackedArtistsCount;

    [ObservableProperty]
    private ObservableCollection<ArtistViewModel> trackedArtists = new();

    [ObservableProperty]
    private bool inProgress = false;

    [ObservableProperty]
    private bool isActiveProvider;

    async partial void OnIsActiveProviderChanged(bool oldValue, bool newValue)
    {
        if (!newValue || isLoaded)
        {
            return;
        }

        await RefreshProviderSource();
    }

    private async Task RefreshProviderSource()
    {
        var providerData = await dbContext.MusicProviders
            .Include("Artists")
            .FirstOrDefaultAsync(p => p.MusicProviderId == this.MusicProviderId);

        foreach (var artist in providerData.Artists)
        {
            var artistVm = artistVmFactory.Create();
            artistVm.Initialize(this,
                artist.ArtistId,
                artist.Name.ToDisplayName(),
                artist.Image,
                artist.Uri);

            App.Current.Dispatcher.InvokeAsync(() => TrackedArtists.Add(artistVm));
        }

        isLoaded = true;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditArtistCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteArtistCommand))]
    private ArtistViewModel? selectedArtist;

    private bool IsArtistSelected => SelectedArtist != null;

    public int TrackedArtistsCount => TrackedArtists.Count == 0 ? initialTrackedArtistsCount : TrackedArtists.Count;

    public MusicProviderViewModel(MusicWatcherDbContext dbContext,
        IDialogWindowService dialogWindowService,
        IToastsNotifier toasts,
        ViewModelFactory<ArtistViewModel> artistVmFactory)
    {
        this.dbContext = dbContext;
        this.dialogWindowService = dialogWindowService;
        this.toasts = toasts;
        this.artistVmFactory = artistVmFactory;

        trackedArtists.CollectionChanged += (o, e) => OnPropertyChanged(nameof(TrackedArtistsCount));
    }

    [RelayCommand(CanExecute = nameof(IsArtistSelected))]
    private void EditArtist()
    {
        dialogWindowService.ShowEditArtistWindow(this, SelectedArtist!);
    }

    [RelayCommand(CanExecute = nameof(IsArtistSelected))]
    private async Task DeleteArtist()
    {
        var dialogResult = MessageBox.Show($"Удалить '{SelectedArtist!.Name}' ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (dialogResult == MessageBoxResult.Yes)
        {

            dbContext.Artists.Remove(dbContext.Artists.Find(SelectedArtist.ArtistId)!);
            await dbContext.SaveChangesAsync();

            toasts.ShowSuccess($"Исполнитель удален из списка на отслеживание");
            TrackedArtists.Remove(SelectedArtist);
            SelectedArtist = null;
        }
    }

    public void Initialize(MusicProviderBase template, string image, string uri, int trackedArtistsCount)
    {
        if (isInitialized)
        {
            throw new Exception("MusicProviderViewModel already initialized");
        }

        Template = template;
        Image = image;
        Uri = uri;
        initialTrackedArtistsCount = trackedArtistsCount;

        isInitialized = true;
    }
}