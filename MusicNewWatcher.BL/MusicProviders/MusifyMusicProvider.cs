using MusicNewsWatcher.Core;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Dto;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace MusicNewWatcher.BL.MusicProviders;

public sealed class MusifyMusicProvider : MusicProviderBase
{
    private const string HOST = "https://musify.club";
    private const string AlbumsXPath = "//div[@id='divAlbumsList']/div";
    private readonly HttpClient searchClient;

    public MusifyMusicProvider() : base(2, "Musify")
    {
        searchClient = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            CookieContainer = new CookieContainer(),
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            UseCookies = true
        });

        searchClient.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
        searchClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        searchClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        searchClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        searchClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
        searchClient.DefaultRequestHeaders.Add("User-Agent", "insomnia/2022.6.0");
        searchClient.DefaultRequestHeaders.Add("Referer", HOST);
    }

    public override async Task<AlbumEntity[]> GetAlbumsAsync(ArtistEntity artist)
    {
        var doc = await GetDocument(artist.Uri + "/releases");

        if (doc == null || doc.DocumentNode.SelectSingleNode(AlbumsXPath) == null)
        {
            return Enumerable.Empty<AlbumEntity>().ToArray();
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

        if (parsedAlbums.Length > 0)
        {
            AlbumEntity[] albums = new AlbumEntity[parsedAlbums.Length];

            for (int i = 0; i < parsedAlbums.Length; i++)
            {
                albums[i] = new AlbumEntity()
                {
                    ArtistId = artist.ArtistId,
                    Title = parsedAlbums[i].Title,
                    Image = parsedAlbums[i].Image,
                    Uri = parsedAlbums[i].Uri
                };

                if (DateTime.TryParseExact(parsedAlbums[i].DateTimeString, "dd.MM.yyyy", null, DateTimeStyles.None, out var created))
                {
                    albums[i].Created = created;
                }
            }

            return albums;
        }


        return Enumerable.Empty<AlbumEntity>().ToArray();
    }

    public override async Task<TrackEntity[]> GetTracksAsync(AlbumEntity album)
    {
        if (!string.IsNullOrWhiteSpace(album.Uri) && album.Uri.Contains(HOST))
        {
            var doc = await GetDocument(album.Uri);
            const string trackXPath = "//div[contains(@id, 'playerDiv')]";

            if (doc != null && doc.DocumentNode.SelectSingleNode(trackXPath) != null)
            {
                var tracks = doc.DocumentNode.SelectNodes(trackXPath)
                    .Select(div => new TrackEntity()
                    {
                        Name = div.SelectSingleNode(".//div[@class='playlist__heading']/a[last()]").InnerText?.Trim() ?? "<Ошибка>",
                        AlbumId = album.AlbumId,
                        DownloadUri = HOST + div.SelectSingleNode(".//a[@itemprop='audio' and @href]")?.GetAttributeValue("href", string.Empty)
                    }).ToArray();

                return tracks;
            }
        }
        return Enumerable.Empty<TrackEntity>().ToArray();
    }

    public override async Task<ArtistEntity[]> SerchArtist(string searchText)
    {
        string uri = $"{HOST}/search/suggestions?term={HttpUtility.UrlEncode(searchText)}";

        try
        {
            var response = await searchClient.GetFromJsonAsync<MusifySearchResultDto[]>(uri);
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


        return await base.SerchArtist(searchText);
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
