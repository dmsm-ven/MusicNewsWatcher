namespace MusicNewsWatcher.Desktop.Services;

public interface IToastsNotifier
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInformation(string message);
}

public class MockToastsNotifier : IToastsNotifier
{
    public void ShowError(string message)
    {
        //Do nothing
    }

    public void ShowInformation(string message)
    {
        //Do nothing
    }

    public void ShowSuccess(string message)
    {
        //Do nothing
    }
}