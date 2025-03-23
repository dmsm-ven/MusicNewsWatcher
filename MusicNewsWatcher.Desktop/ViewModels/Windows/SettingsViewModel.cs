using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.ViewModels;

public partial class SettingsWindowViewModel : ObservableValidator
{
    private readonly MusicWatcherDbContext dbContext;

    [ObservableProperty]
    [Range(30, int.MaxValue)]
    private int updateManagerIntervalInMinutes;

    [ObservableProperty]
    [Range(1, 32)]
    private int downloadThreadsNumber;

    [ObservableProperty]
    private string telegramApiToken;

    [ObservableProperty]
    private string telegramChatId;

    public SettingsWindowViewModel(MusicWatcherDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [RelayCommand]
    private async Task Loaded()
    {
        var pi = this.GetType().GetProperties();

        var settings = await dbContext.Settings.ToListAsync();

        foreach (var settingEntity in settings)
        {
            SetProperty(settingEntity.Name, settingEntity.Value);
        }
    }

    private void SetProperty(string name, string value)
    {
        try
        {
            var prop = this.GetType().GetProperty(name);
            TypeCode typeCode = Type.GetTypeCode(prop.PropertyType);
            object? propValue = typeCode switch
            {
                TypeCode.Int32 => int.Parse(value),
                TypeCode.String => value,
                _ => throw new FormatException($"Unsupported type {typeCode}")
            };
            prop.SetValue(this, propValue);
        }
        catch
        {
            throw new FormatException($"Unsupported type {name}");
        }
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        dbContext.Settings.FirstOrDefault(s => s.Name == nameof(this.DownloadThreadsNumber)).Value = this.DownloadThreadsNumber.ToString();
        dbContext.Settings.FirstOrDefault(s => s.Name == nameof(this.TelegramApiToken)).Value = this.TelegramApiToken;
        dbContext.Settings.FirstOrDefault(s => s.Name == nameof(this.TelegramChatId)).Value = this.TelegramChatId;
        dbContext.Settings.FirstOrDefault(s => s.Name == nameof(this.UpdateManagerIntervalInMinutes)).Value = this.UpdateManagerIntervalInMinutes.ToString();

        await dbContext.SaveChangesAsync();
    }
}
