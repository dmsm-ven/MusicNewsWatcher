using MusicNewsWatcher.API.MusicProviders;
using MusicNewsWatcher.API.MusicProviders.Base;

namespace MusicNewsWatcher.API.Services;

public class CrawlerHttpClientProviderFactory
{
    private IReadOnlyDictionary<Type, HttpClient> httpClientsMap;
    public CrawlerHttpClientProviderFactory()
    {
        var dic = new Dictionary<Type, HttpClient>();
        var defaultClient = new HttpClient();
        dic[typeof(BandcampMusicProvider)] = defaultClient;

        var musifyHttpClient = new HttpClient(new HttpClientHandler()
        {
            UseCookies = true,
            CookieContainer = new(),
            AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
        });
        musifyHttpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        musifyHttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36");
        musifyHttpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
        musifyHttpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        musifyHttpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        musifyHttpClient.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
        musifyHttpClient.DefaultRequestHeaders.Add("sec-fetch-storage-access", "active");
        musifyHttpClient.DefaultRequestHeaders.Add("Origin", "https://musify.club");
        dic[typeof(MusifyMusicProvider)] = musifyHttpClient;

        httpClientsMap = dic;
    }

    public HttpClient GetClientForProvider(MusicProviderBase provider)
    {
        var type = provider.GetType();
        if (httpClientsMap.ContainsKey(type))
        {
            return httpClientsMap[type];
        }
        throw new NotSupportedException();
    }
}