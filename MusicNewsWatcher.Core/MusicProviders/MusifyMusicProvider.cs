using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
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
        var doc = await GetDocument(artist.Uri +"/releases");

        if (doc != null && doc.DocumentNode.SelectSingleNode(AlbumsXPath) != null)
        {
            var albums = doc.DocumentNode
                .SelectNodes("//div[@id='divAlbumsList']/div")
                .Select(div => new AlbumEntity()
                {
                    ArtistId = artist.ArtistId,
                    Title = div.SelectSingleNode("./a/img").GetAttributeValue("alt", string.Empty),
                    Image = div.SelectSingleNode("./a/img").GetAttributeValue("data-src", String.Empty),
                    Uri = HOST + div.SelectSingleNode("./a").GetAttributeValue("href", String.Empty),
                    Created = DateTime.Parse(div.SelectSingleNode(".//i[contains(@class, 'zmdi-calendar')]").ParentNode.InnerText.Trim())
                }).ToArray();

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
}
