using MusicNewsWatcher.Core.Models;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.Core.Interfaces;

public interface IMusicNewsCrawler
{
    Task<IReadOnlyList<AlbumDto>> CheckUpdatesForArtistAndSaveIfHasAsync(int providerId, int artistId);
    Task CheckUpdatesForAlbumAsync(int albumId);
    Task<IReadOnlyList<NewAlbumFoundResult>> CheckUpdatesAllAsync(CancellationToken stoppingToken);

}
