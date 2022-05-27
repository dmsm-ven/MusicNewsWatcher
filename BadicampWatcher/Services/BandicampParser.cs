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
    private readonly BandcampWatcherDbContext dbContext;

    public BandcampParser(BandcampWatcherDbContext dbContext)
    {
        client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            CookieContainer = new System.Net.CookieContainer(),
            UseCookies = true,
            AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
        });
        this.dbContext = dbContext;
    }

    public async Task CheckUpdates(IList<ArtistModel> artists, int chunkSize = 5)
    {
        int totalArtists = artists.Count;
        int currentPage = 1;

        do
        {
            var tasks = artists
                .Select(a => CheckArtistNewAlbums(a))
                .Skip((currentPage - 1) * chunkSize)
                .Take(chunkSize)
                .ToArray();
            await Task.WhenAll(tasks);
            currentPage++;
        } while (chunkSize * (currentPage - 1) < totalArtists);
    }

    private async Task CheckArtistNewAlbums(ArtistModel artist)
    {
        artist.CheckInProgress = true;

        try
        {
            var page = await client.GetStringAsync(artist.Uri + "/music");

            var doc = new HtmlDocument();
            doc.LoadHtml(page);

            artist.HasNew = false;
            int beforeCount = artist.Albums.Count;
            
            doc.DocumentNode.SelectNodes("//ol[@id='music-grid']/li")
                .Select(li => new AlbumModel(dbContext)
                {
                    Title = li.SelectSingleNode(".//p[@class='title']")?.InnerText.Trim() ?? "<Нет названия>",
                    Created = DateTime.Now,
                    Uri = $"{artist.Uri.Trim('/')}{li.SelectSingleNode(".//a").GetAttributeValue("href", null)}",
                    Image = li.SelectSingleNode(".//img").GetAttributeValue("src", null)
                })
                .Where(na => artist.Albums.FirstOrDefault(a => a.Uri == na.Uri) == null)
                .ToList()
                .ForEach(na => artist.Albums.Insert(0, na));

            artist.HasNew = beforeCount != artist.Albums.Count;
        }
        catch
        {
            
        }
        finally
        {
            artist.CheckInProgress = false;
        }
    }
}