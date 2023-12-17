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
