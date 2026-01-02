using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MusicNewsWatcher.Desktop.ViewModels.Items;

public class AlbumDownloadSelectionChangedMessage : ValueChangedMessage<AlbumViewModel>
{
    public AlbumDownloadSelectionChangedMessage(AlbumViewModel album): base(album)
    {
        
    }
}
