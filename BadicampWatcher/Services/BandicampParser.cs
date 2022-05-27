using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BandcampWatcher.ViewModels;

internal class BandcampParser
{
    private readonly HttpClient client;
    public BandcampParser()
    {
        client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            CookieContainer = new System.Net.CookieContainer(),
            UseCookies = true,
            AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
        });
    }

    internal async Task<Dictionary<ArtistModel, List<AlbumModel>>> CheckUpdates(IEnumerable<ArtistModel>  artists, IProgress<int> progress)
    {
        var dic = new Dictionary<ArtistModel, List<AlbumModel>>();

        int current = 0;

        foreach(var artist in artists)
        {
            try
            {
                progress?.Report(current);
                await GetArtistAlbumsInfo(dic, artist);
                current++;
                progress?.Report(current);
            }
            catch
            {

            }

            
        }

        return dic;
    }


    private async Task GetArtistAlbumsInfo(Dictionary<ArtistModel, List<AlbumModel>> dic, ArtistModel artist)
    {
        var page = await client.GetStringAsync(artist.Uri + "/music");

        var doc = new HtmlDocument();
        doc.LoadHtml(page);

        var findedAlbums = doc.DocumentNode.SelectNodes("//ol[@id='music-grid']/li")
            .Select(li => new AlbumModel()
            {
                Title = li.SelectSingleNode(".//p[@class='title']")?.InnerText.Trim() ?? "<Нет названия>",
                Created = DateTime.Now,
                Uri = $"{artist.Uri.Trim('/')}{li.SelectSingleNode(".//a").GetAttributeValue("href", null)}",
                Image = li.SelectSingleNode(".//img").GetAttributeValue("src", null),
            })
            .ToList();

        foreach (var album in findedAlbums)
        {
            if (artist.Albums.FirstOrDefault(a => a.Uri.Equals(album.Uri)) != null) { continue; }

            if (!dic.ContainsKey(artist))
            {
                dic[artist] = new List<AlbumModel>();
            }
            dic[artist].Add(album);
        }
    }
}