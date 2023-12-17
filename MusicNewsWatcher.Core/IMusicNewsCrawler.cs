
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewsWatcher.Core;

public interface IMusicNewsCrawler
{
    Task CheckUpdatesAllAsync(IEnumerable<MusicProviderBase> musicProviders);
    Task CheckUpdatesForAlbumAsync(MusicProviderBase provider, int albumId);
    Task<AlbumEntity[]> CheckUpdatesForArtistAndSaveIfHasAsync(MusicProviderBase provider, int artistId);
}