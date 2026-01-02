using MusicNewsWatcher.Desktop.Interfaces;

namespace MusicNewsWatcher.Desktop.Services;

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