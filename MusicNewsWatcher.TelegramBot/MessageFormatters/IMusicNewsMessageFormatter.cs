using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.Dto;

namespace MusicNewsWatcher.TelegramBot.MessageFormatters;

public interface IMusicNewsMessageFormatter
{
    string BuildNewAlbumsFoundMessage(string providerName, ArtistDto artist, IEnumerable<AlbumDto> albums);
}
