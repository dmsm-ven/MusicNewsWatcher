using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;

public class LambdaCommand : Command
{
    private readonly Action<object> action;
    private readonly Func<object, bool> canExecute;
    private readonly ICommand? cancelDownloading;
    private readonly Func<object, bool> p;

    public LambdaCommand(Action<object> action, Func<object, bool>? canExecute = null)
    {
        this.action = action ?? throw new ArgumentNullException(nameof(action));
        this.canExecute = canExecute;
    }

    public LambdaCommand(ICommand? cancelDownloading, Func<object, bool> p)
    {
        this.cancelDownloading = cancelDownloading;
        this.p = p;
    }

    public override bool CanExecute(object parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    public override void Execute(object parameter)
    {
        action(parameter);
    }
}
