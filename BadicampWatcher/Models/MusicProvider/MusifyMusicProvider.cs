using BandcampWatcher.DataAccess;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandcampWatcher.Models;

public class MusifyMusicProvider : MusicProviderBase
{
    const string HOST = "https://musify.club";

    public MusifyMusicProvider() : base(2, "Musify") { }

    public override async Task<List<AlbumEntity>> GetAlbums(ArtistEntity artist)
    {
        var page = await client.GetStringAsync(artist.Uri + "/releases");

        var doc = new HtmlDocument();
        doc.LoadHtml(page);


        if(doc.DocumentNode.SelectSingleNode("//div[@id='divAlbumsList']/div") == null) { return new(); }

        var albums = doc.DocumentNode
            .SelectNodes("//div[@id='divAlbumsList']/div")
            .Select(div => new AlbumEntity()
            {
                ArtistId = artist.ArtistId,
                Title = div.SelectSingleNode("./a/img").GetAttributeValue("alt", string.Empty),
                Image = div.SelectSingleNode("./a/img").GetAttributeValue("data-src", String.Empty),
                Uri = HOST + div.SelectSingleNode("./a").GetAttributeValue("href", String.Empty)
            }).ToList();

        return albums;
    }
}
