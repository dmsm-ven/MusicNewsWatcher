using BandcampWatcher.DataAccess;
using System.Collections.Generic;
using System.Net.Http;
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
