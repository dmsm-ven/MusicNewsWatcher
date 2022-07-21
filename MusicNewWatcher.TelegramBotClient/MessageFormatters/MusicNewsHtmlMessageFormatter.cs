using System.Collections.Generic;
using System.Text;

namespace MusicNewsWatcher.TelegramBot;

public class MusicNewsHtmlMessageFormatter : IMusicNewsMessageFormatter
{
    public string NewAlbumsFoundMessage(string provider, (string, string) artistNameWithUri, IEnumerable<(string, string)> albumNamesWithUrl)
    {
        var sb = new StringBuilder()
            .AppendLine($"На сайте <b>{provider}</b> появились альбомы для отслеживаемого исполнителя")
            .AppendLine($"<a href=\"{artistNameWithUri.Item2}\"><b>{artistNameWithUri.Item1}</b></a>");

        foreach (var album in albumNamesWithUrl)
        {
            sb.AppendLine($"- <a href=\"{album.Item2}\" target=\"_blank\">{album.Item1}</a>");
        }
        string message = sb.ToString();

        return message;
    }
}
