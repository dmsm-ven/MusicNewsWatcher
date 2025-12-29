using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models;

namespace MusicNewsWatcher.Core.Interfaces;

public interface IMusicNewsCrawler
{
    Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId);
    Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders, CancellationToken stoppingToken);
    Task<IReadOnlyList<AlbumEntity>> CheckUpdatesForArtistAndSaveIfHasAsync(MusicProviderBase provider, int artistId);
}
