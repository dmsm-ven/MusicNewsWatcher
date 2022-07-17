using BandcampWatcher.DataAccess;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BandcampWatcher.Models;

public abstract class MusicProviderBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    protected HttpClient client;

    public abstract Task<List<AlbumEntity>> GetAlbums(ArtistEntity artist);

    protected MusicProviderBase(int id, string name)
    {
        Id = id;
        Name = name;
        client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            CookieContainer = new System.Net.CookieContainer(),
            UseCookies = true
        });

    }
}

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
                Image = li.SelectSingleNode(".//img").GetAttributeValue("src", null)
            })
            .ToList();

        return albums;
    }
}

public class MusifyMusicProvider : MusicProviderBase
{
    public MusifyMusicProvider() : base(2, "Musify") { }

    public override Task<List<AlbumEntity>> GetAlbums(ArtistEntity artist)
    {
        throw new NotImplementedException();
    }
}
