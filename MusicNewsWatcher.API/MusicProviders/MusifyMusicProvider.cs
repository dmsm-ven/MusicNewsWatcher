using HtmlAgilityPack;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.Models.Dtos;
using System.Globalization;

namespace MusicNewsWatcher.API.MusicProviders;

public sealed class MusifyMusicProvider : MusicProviderBase
{
    private const string HOST = "https://musify.club";
    private const string AlbumsXPath = "//div[@id='divAlbumsList']/div";
    public override string AlbumsUriPath(ArtistEntity artist)
    {
        return $"{artist.Uri}/releases";
    }
    public MusifyMusicProvider()
    {
        this.Id = 2;
        this.Name = "Musify";
    }
    public override Task<AlbumDto[]> GetAlbumsAsync(ArtistEntity artist, HtmlDocument doc)
    {
        // SelectNodes возвращает null, если ничего не найдено
        var albumNodes = doc?.DocumentNode?.SelectNodes(AlbumsXPath);
        if (albumNodes == null)
        {
            return Task.FromResult(Array.Empty<AlbumDto>());
        }

        var albums = new List<AlbumDto>();

        foreach (var div in albumNodes)
        {
            // Любое отклонение разметки одного альбома пропускает только этот альбом, а не весь разбор артиста
            var link = div.SelectSingleNode("./a");
            var img = link?.SelectSingleNode("./img");
            string href = link?.GetAttributeValue("href", string.Empty).Trim() ?? string.Empty;

            if (img == null || string.IsNullOrEmpty(href))
            {
                continue;
            }

            if (!int.TryParse(div.GetAttributeValue("data-type", "0"), out int albumType) ||
                !Enum.IsDefined(typeof(MusifyAlbumType), albumType))
            {
                continue;
            }

            string title = img.GetAttributeValue("alt", string.Empty).Trim();
            string image = img.GetAttributeValue("data-src", string.Empty).Trim();
            string uri = href.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? href : HOST + href;

            string dateTimeString = div.SelectSingleNode(".//i[contains(@class, 'zmdi-calendar')]")?.ParentNode?.InnerText?.Trim() ?? string.Empty;
            if (!DateTime.TryParseExact(dateTimeString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var created))
            {
                created = DateTime.MinValue;
            }

            albums.Add(new AlbumDto(0, artist.ArtistId, title, created, false, image, uri));
        }

        return Task.FromResult(albums.ToArray());
    }

    public override Task<TrackDto[]> GetTracksAsync(AlbumEntity album, HtmlDocument doc)
    {
        if (string.IsNullOrWhiteSpace(album.Uri) || !album.Uri.Contains(HOST))
        {
            return Task.FromResult(Array.Empty<TrackDto>());
        }

        var trackNodes = doc?.DocumentNode?.SelectNodes("//div[contains(@id, 'playerDiv')]");
        if (trackNodes == null)
        {
            return Task.FromResult(Array.Empty<TrackDto>());
        }

        var tracks = trackNodes
            .Select(div =>
            {
                string playUrl = div.SelectSingleNode(".//div[@data-play-url]")?.GetAttributeValue("data-play-url", string.Empty) ?? string.Empty;

                return new TrackDto(Id: 0,
                    AlbumId: album.AlbumId,
                    Name: div.SelectSingleNode(".//div[@class='playlist__heading']/a[last()]")?.InnerText?.Trim() ?? "<Ошибка>",
                    // Если у трека нет ссылки на файл, не подставляем "голый" адрес сайта
                    DownloadUri: string.IsNullOrWhiteSpace(playUrl) ? null : HOST + playUrl);
            })
            .ToArray();

        return Task.FromResult(tracks);
    }
    public override Task<AlbumDto[]> SerchArtist(string searchText)
    {
        return base.SerchArtist(searchText);
        /*
        string uri = $"{HOST}/search/suggestions?term={HttpUtility.UrlEncode(searchText)}";

        try
        {
            var response = await this.httpClient.GetFromJsonAsync<MusifySearchResultDto[]>(uri);
            if (response != null && response.Length > 0)
            {
                var data = response
                    .Where(i => i.Category == "Исполнители")
                    .Take(10)
                    .Select(i => new ArtistEntity()
                    {
                        Name = i.Label,
                        Image = i.Image,
                        Uri = HOST + i.Uri
                    })
                    .ToArray();

                return data;
            }
        }
        catch
        {
            //ignore search errors
        }  
        */
    }

    private enum MusifyAlbumType
    {
        Album = 2,//Студийный альбом = 2,
        EP = 3,//EP = 3,
        Single = 4,//Сингл = 4,
        Split = 12,//Сплиты = 12,
        Soundtrack = 14//Саундтреки = 14,
    }
}
