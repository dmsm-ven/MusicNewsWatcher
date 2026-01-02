using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.MusicProviders.Base;
using System.Web;
namespace MusicNewsWatcher.API.MusicProviders;

public sealed class BandcampMusicProvider : MusicProviderBase
{
    const string albumsXPath = "//ol[@id='music-grid']/li";
    const string HOST = "bandcamp.com";

    public BandcampMusicProvider(HttpClient client) : base(client)
    {
        this.Id = 1;
        this.Name = "Bandcamp";
    }

    public override async Task<AlbumEntity[]> GetAlbumsAsync(ArtistEntity artist)
    {
        var doc = await GetDocument(artist.Uri + "/music");

        if (doc != null && doc.DocumentNode.SelectSingleNode(albumsXPath) != null)
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
        return Enumerable.Empty<AlbumEntity>().ToArray();
    }

    public override async Task<TrackEntity[]> GetTracksAsync(AlbumEntity album)
    {
        if (!string.IsNullOrWhiteSpace(album.Uri) && album.Uri.Contains(HOST))
        {
            var doc = await GetDocument(album.Uri);
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
        return Enumerable.Empty<TrackEntity>().ToArray();
    }

}
