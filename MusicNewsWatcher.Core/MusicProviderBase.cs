using HtmlAgilityPack;
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewsWatcher.Core;

public abstract class MusicProviderBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    protected HttpClient client;

    public abstract Task<AlbumEntity[]> GetAlbumsAsync(ArtistEntity artist);
    public abstract Task<TrackEntity[]> GetTracksAsync(AlbumEntity album);

    //Переопределяем метод если провайдера позволяет делать поиск по сайту, иначе просто возвращаем пустой массив
    public virtual Task<ArtistEntity[]> SerchArtist(string searchText)
    {
        return Task.FromResult(Enumerable.Empty<ArtistEntity>().ToArray());
    }

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

        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        //client.DefaultRequestHeaders.Add("Accept-encoding", "gzip, deflate, br, zstd");
        client.DefaultRequestHeaders.Add("Accept-language", "en-US,en;q=0.9,ru-RU;q=0.8,ru;q=0.7");
        client.DefaultRequestHeaders.Add("cache-control", "no-cache");
        client.DefaultRequestHeaders.Add("pragma", "no-cache");
        client.DefaultRequestHeaders.Add("priority", "u=0, i");
        client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"");
        client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        client.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
        client.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
        client.DefaultRequestHeaders.Add("sec-fetch-site", "none");
        client.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
        client.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
    }

    protected async Task<HtmlDocument?> GetDocument(string uri)
    {
        try
        {
            var page = await client.GetStringAsync(uri);
            var doc = new HtmlDocument();
            doc.LoadHtml(page);
            return doc;
        }
        catch
        {
            return null;
        }
    }

}
