using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Core;

public sealed class MusifyMusicProvider : MusicProviderBase
{
    const string HOST = "https://musify.club";
    const string AlbumsXPath = "//div[@id='divAlbumsList']/div";

    public MusifyMusicProvider() : base(2, "Musify") { }

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
                Image = div.SelectSingleNode("./a/img").GetAttributeValue("data-src", String.Empty).Trim(),
                Uri = HOST + div.SelectSingleNode("./a").GetAttributeValue("href", String.Empty).Trim(),
                DateTimeString = div.SelectSingleNode(".//i[contains(@class, 'zmdi-calendar')]").ParentNode.InnerText.Trim(),
                AlbumType = int.Parse(div.GetAttributeValue("data-type", "0"))

            })
            .Where(a => Enum.IsDefined(typeof(MusifyAlbumType), a.AlbumType))
            .ToArray();

        if (parsedAlbums.Length > 0)
        {
            AlbumEntity[] albums = new AlbumEntity[parsedAlbums.Length];

            for(int i = 0; i< parsedAlbums.Length; i++)
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

    enum MusifyAlbumType 
    {
        Album = 2,//Студийный альбом = 2,
        EP = 3,//EP = 3,
        Single = 4,//Сингл = 4,
        Split = 12,//Сплиты = 12,
        Soundtrack = 14//Саундтреки = 14,
    }
}
