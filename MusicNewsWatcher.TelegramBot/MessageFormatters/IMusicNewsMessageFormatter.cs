using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.TelegramBot.MessageFormatters;

public interface IMusicNewsMessageFormatter
{
    string BuildNewAlbumsFoundMessage(string providerName, ArtistDto artist, IEnumerable<AlbumDto> albums);

    string BuildTrackedArtistsListMessage(IReadOnlyDictionary<string, string[]> providerToArtistMap);
}
