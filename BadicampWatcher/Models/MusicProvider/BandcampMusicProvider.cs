using BandcampWatcher.DataAccess;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandcampWatcher.Models;

public class BandcampMusicProvider : MusicProviderBase
{
    public BandcampMusicProvider() : base(1, "Bandcamp") { }

    public override async Task<List<AlbumEntity>> GetAlbums(ArtistEntity artist)
    {
        var page = await client.GetStringAsync(artist.Uri + "/music");

        var doc = new HtmlDocument();
        doc.LoadHtml(page);

        int beforeCount = artist.Albums.Count;

        var albums = doc.DocumentNode.SelectNodes("//ol[@id='music-grid']/li")
            .Select(li => new AlbumEntity()
            {
                Title = li.SelectSingleNode(".//p[@class='title']")?.InnerText.Trim() ?? "<Нет названия>",
                Created = DateTime.Now,
                Uri = $"{artist.Uri.Trim('/')}{li.SelectSingleNode(".//a").GetAttributeValue("href", null)}",
                Image = li.SelectSingleNode(".//img").GetAttributeValue("src", null),
                ArtistId = artist.ArtistId
            })
            .ToList();

        return albums;
    }
}
