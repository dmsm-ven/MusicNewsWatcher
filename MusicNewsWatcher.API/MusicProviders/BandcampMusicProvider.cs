using HtmlAgilityPack;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.Models.Dtos;
namespace MusicNewsWatcher.API.MusicProviders;

public sealed class BandcampMusicProvider : MusicProviderBase
{
    const string albumsXPath = "//ol[@id='music-grid']/li";
    const string HOST = "bandcamp.com";

    public BandcampMusicProvider()
    {
        this.Id = 1;
        this.Name = "Bandcamp";
    }

    public override async Task<AlbumDto[]> GetAlbumsAsync(ArtistEntity artist, HtmlDocument doc)
    {
        /*if (doc != null && doc.DocumentNode.SelectSingleNode(albumsXPath) != null)
        {
            var albums = doc.DocumentNode.SelectNodes(albumsXPath)
                .Select(li => new AlbumEntity()
                {
                    Title = li.SelectSingleNode(".//p[@class='title']")?.InnerText.Trim() ?? "<Нет названия>",
                    Created = DateTime.Now,
                    Uri = $"{artist.Uri.Trim('/')}{li.SelectSingleNode(".//a").GetAttributeValue("href", null)}",
                    Image = li.SelectSingleNode(".//img").GetAttributeValue("src", null),
                    ArtistId = artist.ArtistId
                })
                .ToArray();

            return albums;
        }
        */
        return Array.Empty<AlbumDto>();
    }

    public override async Task<TrackDto[]> GetTracksAsync(AlbumEntity album, HtmlDocument doc)
    {
        return Array.Empty<TrackDto>();
        /*
        if (!string.IsNullOrWhiteSpace(album.Uri) && album.Uri.Contains(HOST))
        {
            const string trackXPath = "//table[@id='track_table']//tr[contains(@rel, 'tracknum')]";

            if (doc != null && doc.DocumentNode.SelectSingleNode(trackXPath) != null)
            {
                var tracks = doc.DocumentNode.SelectNodes(trackXPath)
                    .Select(div => new TrackEntity()
                    {
                        Name = div.SelectSingleNode(".//span[@class='track-title']").InnerText?.Trim() ?? "<Ошибка>",
                        AlbumId = album.AlbumId,
                        DownloadUri = null
                    }).ToArray();

                var script = doc.DocumentNode.SelectSingleNode("/html/head/script[last()]");

                if (script != null)
                {
                    var attributeData = HttpUtility.HtmlDecode(script.GetAttributeValue("data-tralbum", string.Empty));
                    if (!string.IsNullOrWhiteSpace(attributeData))
                    {
                        //var jsonRoot = JsonConvert.DeserializeObject<dynamic>(attributeData);
                        //var trackInfo = jsonRoot.trackinfo;
                    }
                }

                return tracks;
            }
        }
        */

    }

}
