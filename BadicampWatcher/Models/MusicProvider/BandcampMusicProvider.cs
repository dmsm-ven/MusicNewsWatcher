using MusicNewsWatcher.DataAccess;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Models;

public class BandcampMusicProvider : MusicProviderBase
{
    const string albumsXPath = "//ol[@id='music-grid']/li";

    public BandcampMusicProvider() : base(1, "Bandcamp") { }

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
        await Task.Delay(TimeSpan.FromSeconds(2));

        var testData = Enumerable.Range(1, 12)
            .Select(i => new TrackEntity()
            {
                Name = $"Track name {i}",
                AlbumId = album.AlbumId
            })
            .ToArray();

        return testData;
    }
}
