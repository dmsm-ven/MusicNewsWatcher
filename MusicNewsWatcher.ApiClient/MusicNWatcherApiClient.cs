using MusicNewsWatcher.Core.Models.Dtos;
using System.Net.Http.Json;

namespace MusicNewsWatcher.ApiClient;

public class MusicNewsWatcherApiClient
{
    private readonly HttpClient client;
    public MusicNewsWatcherApiClient(HttpClient client)
    {
        this.client = client;
    }
    public async Task<MusicProviderDto[]> GetProvidersAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync("api/providers", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return Array.Empty<MusicProviderDto>();
        }
        var providers = await response.Content.ReadFromJsonAsync<MusicProviderDto[]>(cancellationToken: cancellationToken);
        return providers ?? Array.Empty<MusicProviderDto>();
    }
    public async Task<ArtistDto[]> GetProviderArtistsAsync(int providerId, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync($"api/providers/{providerId}/artists", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return Array.Empty<ArtistDto>();
        }
        var artists = await response.Content.ReadFromJsonAsync<ArtistDto[]>(cancellationToken: cancellationToken);
        return artists ?? Array.Empty<ArtistDto>();
    }
    public async Task<AlbumDto[]> GetArtistAlbumsAsync(int providerId, int artistId, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync($"api/providers/{providerId}/artists/{artistId}/albums", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return Array.Empty<AlbumDto>();
        }
        var albums = await response.Content.ReadFromJsonAsync<AlbumDto[]>(cancellationToken: cancellationToken);
        return albums ?? Array.Empty<AlbumDto>();
    }
    public async Task<TrackDto[]> GetAlbumTracksAsync(int providerId, int artistId, int albumId, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync($"api/providers/{providerId}/artists/{artistId}/albums/{albumId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return Array.Empty<TrackDto>();
        }
        var tracks = await response.Content.ReadFromJsonAsync<TrackDto[]>(cancellationToken: cancellationToken);
        return tracks ?? Array.Empty<TrackDto>();
    }

    public async Task<AlbumDto[]?> CheckUpdatesForArtist(int providerId, int artistId, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsync($"api/providers/{providerId}/artists/{artistId}/albums/refresh", new StringContent(""), cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return Array.Empty<AlbumDto>();
        }
        var newAlbums = await response.Content.ReadFromJsonAsync<AlbumDto[]>(cancellationToken: cancellationToken);
        return newAlbums ?? Array.Empty<AlbumDto>();
    }
}
