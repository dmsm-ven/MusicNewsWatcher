using MusicNewsWatcher.Core.Models;

namespace MusicNewsWatcher.TelegramBot.MessageFormatters;

public interface IMusicNewsMessageFormatter
{
    string BuildNewAlbumsFoundMessage(string providerName, ArtistDto artist, IEnumerable<AlbumDto> albums);
}
