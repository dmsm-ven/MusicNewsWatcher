using MusicNewsWatcher.Desktop.Interfaces;
using System.Diagnostics;
using System.Windows;

namespace MusicNewsWatcher.Desktop.Services;

/// <summary>
/// Простейший уведомлятель: ошибки показывает окном сообщения (по одному за раз, без лавины окон),
/// информационные сообщения пишет в Debug.
/// </summary>
public class MessageBoxToastsNotifier : IToastsNotifier
{
    private int errorDialogOpened;

    public void ShowError(string message)
    {
        Debug.WriteLine($"[Error] {message}");

        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher == null || Interlocked.Exchange(ref errorDialogOpened, 1) == 1)
        {
            return;
        }

        dispatcher.BeginInvoke(new Action(() =>
        {
            try
            {
                MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                Interlocked.Exchange(ref errorDialogOpened, 0);
            }
        }));
    }

    public void ShowInformation(string message)
    {
        Debug.WriteLine($"[Info] {message}");
    }

    public void ShowSuccess(string message)
    {
        Debug.WriteLine($"[Success] {message}");
    }
}
