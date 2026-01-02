using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicNewsWatcher.Desktop.ViewModels;

public class ViewModelFactory<T> where T : ObservableObject
{
    public T Create()
    {
        var scope = App.HostContainer.Services.CreateScope();

        return scope.ServiceProvider.GetRequiredService<T>();
    }
}



