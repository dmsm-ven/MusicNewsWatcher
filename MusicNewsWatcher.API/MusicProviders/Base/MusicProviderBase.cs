using HtmlAgilityPack;
using MusicNewsWatcher.API.DataAccess.Entity;

namespace MusicNewsWatcher.API.MusicProviders.Base;

public abstract class MusicProviderBase
{
    protected readonly HttpClient httpClient;

    public int Id { get; init; }
    public string Name { get; init; }
    public abstract Task<AlbumEntity[]> GetAlbumsAsync(ArtistEntity artist);
    public abstract Task<TrackEntity[]> GetTracksAsync(AlbumEntity album);

    //Переопределяем метод если провайдера позволяет делать поиск по сайту, иначе просто возвращаем пустой массив
    public virtual Task<ArtistEntity[]> SerchArtist(string searchText)
    {
        return Task.FromResult(Enumerable.Empty<ArtistEntity>().ToArray());
    }
    protected MusicProviderBase(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    protected async Task<HtmlDocument?> GetDocument(string uri)
    {
        try
        {
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                return doc;
            }
        }
        catch
        {
            // ignored
        }
        return null;
    }
}
