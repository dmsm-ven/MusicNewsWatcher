using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.Services;

public interface IToastsNotifier
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInformation(string message);
}

public class DewCrewToastsNotifier : IToastsNotifier
{
    private readonly Notifier notifier;

    public DewCrewToastsNotifier(Notifier notifier)
    {
        this.notifier = notifier;
    }

    public void ShowError(string message)
    {
        notifier.ShowError(message);
    }

    public void ShowInformation(string message)
    {
        notifier.ShowInformation(message);
    }

    public void ShowSuccess(string message)
    {
        notifier.ShowSuccess(message);
    }
}
