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
    public override async Task<AlbumDto[]> GetAlbumsAsync(ArtistEntity artist, HtmlDocument doc)
    {
        if (doc == null || doc.DocumentNode.SelectSingleNode(AlbumsXPath) == null)
        {
            return Array.Empty<AlbumDto>();
        }

        var parsedAlbums = doc.DocumentNode
            .SelectNodes("//div[@id='divAlbumsList']/div")
            .Select(div => new
            {
                Title = div.SelectSingleNode("./a/img").GetAttributeValue("alt", string.Empty).Trim(),
                Image = div.SelectSingleNode("./a/img").GetAttributeValue("data-src", string.Empty).Trim(),
                Uri = HOST + div.SelectSingleNode("./a").GetAttributeValue("href", string.Empty).Trim(),
                DateTimeString = div.SelectSingleNode(".//i[contains(@class, 'zmdi-calendar')]").ParentNode.InnerText.Trim(),
                AlbumType = int.Parse(div.GetAttributeValue("data-type", "0"))

            })
            .Where(a => Enum.IsDefined(typeof(MusifyAlbumType), a.AlbumType))
            .ToArray();

        if (parsedAlbums.Length == 0)
        {
            return Array.Empty<AlbumDto>();
        }

        var albums = new List<AlbumDto>();

        foreach (var album in parsedAlbums)
        {
            if (!DateTime.TryParseExact(album.DateTimeString, "dd.MM.yyyy", null, DateTimeStyles.None, out var created))
            {
                created = DateTime.MinValue;
            }
            albums.Add(new(0, artist.ArtistId, album.Title, created, false, album.Image, album.Uri));
        }

        return albums.ToArray();
    }
    public override async Task<TrackDto[]> GetTracksAsync(AlbumEntity album, HtmlDocument doc)
    {
        if (!string.IsNullOrWhiteSpace(album.Uri) && album.Uri.Contains(HOST))
        {
            const string trackXPath = "//div[contains(@id, 'playerDiv')]";

            if (doc != null && doc.DocumentNode.SelectSingleNode(trackXPath) != null)
            {
                var tracks = doc.DocumentNode.SelectNodes(trackXPath)
                    .Select(div => new TrackDto(Id: 0,
                    AlbumId: album.AlbumId,
                    Name: div.SelectSingleNode(".//div[@class='playlist__heading']/a[last()]").InnerText?.Trim() ?? "<Ошибка>",
                    DownloadUri: HOST + div.SelectSingleNode(".//div[@data-play-url]")?.GetAttributeValue("data-play-url", string.Empty)
                    ))
                    .ToArray();

                return tracks;
            }
        }
        return Array.Empty<TrackDto>();
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
