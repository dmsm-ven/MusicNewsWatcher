using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.TelegramBot.MessageFormatters;

public interface IMusicNewsMessageFormatter
{
    string BuildNewAlbumsFoundMessage(string providerName, ArtistDto artist, IEnumerable<AlbumDto> albums);

    string BuildTrackedArtistsListMessage(IReadOnlyDictionary<string, string[]> providerToArtistMap);
    string BuilderLastAlbumsMessage(LastParsedAlbumInfo[] items);
}

public class LastParsedAlbumInfo()
{
    public string AlbumName { get; init; } = string.Empty;
    public string ArtistName { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.MinValue;
}