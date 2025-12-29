using MusicNewsWatcher.Core.Models.Dtos;
using System.Text;

namespace MusicNewsWatcher.TelegramBot.MessageFormatters;

public class MusicNewsHtmlMessageFormatter : IMusicNewsMessageFormatter
{
    public string BuildNewAlbumsFoundMessage(string provider, ArtistDto artist, IEnumerable<AlbumDto> newAlbums)
    {
        var sb = new StringBuilder()
            .AppendLine($"На сайте <b>{provider}</b> появились альбомы для отслеживаемого исполнителя")
            .AppendLine($"<a href=\"{artist.Uri}\"><b>{artist.Name}</b></a>");

        foreach (var album in newAlbums)
        {
            sb.AppendLine($"- <a href=\"{album.uri}\" target=\"_blank\">{album.name}</a>");
        }
        string message = sb.ToString();

        return message;
    }
}
