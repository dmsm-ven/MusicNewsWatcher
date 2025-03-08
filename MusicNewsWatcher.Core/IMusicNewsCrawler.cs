
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewsWatcher.Core;

public interface IMusicNewsCrawler
{
    Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId);
    Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders, CancellationToken stoppingToken);
    Task<IReadOnlyList<AlbumEntity>> CheckUpdatesForArtistAndSaveIfHasAsync(MusicProviderBase provider, int artistId);
}

public class NewAlbumFoundResult
{
    public string ProviderName { get; init; } = string.Empty;
    public string ArtistName { get; init; } = string.Empty;
    public string ArtistUri { get; init; } = string.Empty;
    public IReadOnlyList<AlbumEntity> Albums { get; init; } = new List<AlbumEntity>();
}