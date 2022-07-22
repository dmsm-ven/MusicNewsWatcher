using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class MusicProviderViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbContextFactory;
    private readonly IToastsNotifier toasts;

    public event Action<MusicProviderViewModel> OnMusicProviderChanged;
    public ObservableCollection<ArtistViewModel> TrackedArtists { get; init; } = new();
    public MusicProviderBase MusicProvider { get; init; }
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
        set
        {
            if (selectedArtist != null)
            {
                selectedArtist.IsActiveArtist = false;
            }

            if (Set(ref selectedArtist, value) && value != null)
            {
                selectedArtist.IsActiveArtist = true;
            }
        }
    }

    public ICommand ChangeSelectedArtistCommand { get; }
    public ICommand CheckUpdatesSelectedCommand { get; }
    public ICommand AddArtistCommand { get; }
    public ICommand EditArtistCommand { get; }
    public ICommand DeleteArtistCommand { get; }
    
    public MusicProviderViewModel()
    {
        dbContextFactory = App.HostContainer.Services.GetRequiredService<IDbContextFactory<MusicWatcherDbContext>>();
        toasts = App.HostContainer.Services.GetRequiredService<IToastsNotifier>();

        ChangeSelectedArtistCommand = new LambdaCommand(e => OnMusicProviderChanged?.Invoke(this));
        AddArtistCommand = new LambdaCommand(async e => await AddArtist());
        DeleteArtistCommand = new LambdaCommand(DeleteArtist, e => SelectedArtist != null);
        EditArtistCommand = new LambdaCommand(EditArtist, e => SelectedArtist != null);
    }

    private async Task AddArtist()
    {
        var dialogVm = new AddNewArtistDialogViewModel(dbContextFactory);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {

        }
    }

    private async void EditArtist(object obj)
    {
        var dialogVm = new AddNewArtistDialogViewModel(dbContextFactory, SelectedArtist!);
        var dialogWindow = new AddNewArtistDialog();
        dialogWindow.DataContext = dialogVm;
        if (dialogWindow.ShowDialog() == true)
        {
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
}
