using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.Models.Dtos;
using System.Text;

namespace MusicNewsWatcher.TelegramBot.MessageFormatters;

public class MusicNewsHtmlMessageFormatter : IMusicNewsMessageFormatter
{
    public string BuilderLastAlbumsMessage(LastParsedAlbumInfo[] items)
    {
        if (items is null)
        {
            return "Пусто";
        }

        var sb = new StringBuilder()
        .AppendLine($"Список последних ({items.Length}) найденных альбомов");

        foreach (var album in items)
        {
            sb.AppendLine($" - {album.CreatedAt.ToRussianLocalTime()} <a href=\"{album.Uri}\" target=\"_blank\">{album.ArtistName} | {album.AlbumName}</a>");
        }
        string message = sb.ToString();

        return message;
    }

    public string BuildNewAlbumsFoundMessage(string provider, ArtistDto artist, IEnumerable<AlbumDto> newAlbums)
    {
        var sb = new StringBuilder()
            .AppendLine($"На сайте <b>{provider}</b> появились альбомы для отслеживаемого исполнителя")
            .AppendLine($"<a href=\"{artist.Uri}\"><b>{artist.Name}</b></a>");

        foreach (var album in newAlbums)
        {
            sb.AppendLine($"- <a href=\"{album.Uri}\" target=\"_blank\">{album.Title}</a>");
        }
        string message = sb.ToString();

        return message;
    }

    public string BuildTrackedArtistsListMessage(IReadOnlyDictionary<string, string[]> providerToArtistMap)
    {
        var sb = new StringBuilder()
    .AppendLine($"Список отслеживаемых провайдеров");

        foreach (var providerGroup in providerToArtistMap)
        {
            if (providerGroup.Value is null || providerGroup.Value.Length == 0)
            {
                continue;
            }
            sb.AppendLine($"<b>{providerGroup.Key}</b> ({providerGroup.Value.Length} исполнителей)");
            foreach (var artist in providerGroup.Value)
            {
                sb.AppendLine($"- {artist}");
            }
        }

        string message = sb.ToString();

        return message;
    }
}
