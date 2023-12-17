using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;
using MusicNewsWatcher.Desktop.ViewModels.Base;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class SettingsWindowViewModel : ViewModelBase
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;
    private int updateManagerIntervalInMinutes = 30;
    public int UpdateManagerIntervalInMinutes
    {
        get => updateManagerIntervalInMinutes;
        set
        {
            bool inBounds = value >= 5;
            int actualValue = inBounds ? value : 5;

            if (Set(ref updateManagerIntervalInMinutes, actualValue) && IsLoaded)
            {
                settingsChanges[nameof(UpdateManagerIntervalInMinutes)] = actualValue.ToString();
                RaisePropertyChanged(nameof(HasChanges));
            }
        }
    }

    private int downloadThreadsNumber = 2;
    public int DownloadThreadsNumber
    {
        get => downloadThreadsNumber;
        set
        {
            bool inBounds = value >= 1 && value <= 32;
            int actualValue = inBounds ? value : 1;

            if (Set(ref downloadThreadsNumber, actualValue) && IsLoaded)
            {
                settingsChanges[nameof(DownloadThreadsNumber)] = actualValue.ToString();
                RaisePropertyChanged(nameof(HasChanges));
            }
        }
    }

    private string telegramApiToken;
    public string TelegramApiToken
    {
        get => telegramApiToken;
        set
        {
            if (Set(ref telegramApiToken, value) && IsLoaded)
            {
                settingsChanges[nameof(TelegramApiToken)] = value;
                RaisePropertyChanged(nameof(HasChanges));
            }
        }
    }

    private string telegramChatId;
    public string TelegramChatId
    {
        get => telegramChatId;
        set
        {
            if (Set(ref telegramChatId, value) && IsLoaded)
            {
                settingsChanges[nameof(TelegramChatId)] = value;
                RaisePropertyChanged(nameof(HasChanges));
            }
        }
    }

    public bool HasChanges => settingsChanges.Values.Any(v => v != null);
    public bool IsLoaded { get; private set; }

    private readonly Dictionary<string, string?> settingsChanges;

    public ICommand SaveCommand { get; }
    public ICommand LoadedCommand { get; }

    public SettingsWindowViewModel()
    {
        LoadedCommand = new LambdaCommand(Loaded);
        SaveCommand = new LambdaCommand(SaveSettings, e => HasChanges);
        settingsChanges = new Dictionary<string, string?>();
    }

    public SettingsWindowViewModel(IDbContextFactory<MusicWatcherDbContext> dbFactory) : this()
    {
        this.dbFactory = dbFactory;
    }

    private void Loaded(object obj)
    {
        var pi = this.GetType().GetProperties();

        using var db = dbFactory.CreateDbContext();

        foreach (var settings in db.Settings)
        {
            var prop = pi.FirstOrDefault(p => p.Name == settings.Name);
            if (prop != null)
            {
                dynamic val = prop.PropertyType == typeof(string) ? settings.Value : int.Parse(settings.Value);
                prop.SetValue(this, val);
                RaisePropertyChanged(prop.Name);
            }
        }
        IsLoaded = true;
    }

    private void SaveSettings(object obj)
    {
        using var db = dbFactory.CreateDbContext();

        foreach (var kvp in settingsChanges.Where(i => i.Value != null))
        {
            var alreadyAddedSetting = db.Settings.FirstOrDefault(i => i.Name.Equals(kvp.Key));
            if (alreadyAddedSetting != null)
            {
                db.Settings.Remove(alreadyAddedSetting);
            }
            db.Settings.Add(new SettingsEntity()
            {
                Name = kvp.Key,
                Value = kvp.Value
            });
            settingsChanges[kvp.Key] = null;
        }
        db.SaveChanges();

        RaisePropertyChanged(nameof(HasChanges));
    }
}
