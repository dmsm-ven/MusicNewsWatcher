namespace MusicNewsWatcher.Desktop.Interfaces;

public interface IToastsNotifier
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInformation(string message);
}
