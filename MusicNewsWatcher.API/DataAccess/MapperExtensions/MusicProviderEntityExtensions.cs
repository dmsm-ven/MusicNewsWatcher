using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.DataAccess.MapperExtensions;

public static class MusicProviderEntityExtensions
{
    public static MusicProviderDto ToDto(this MusicProviderEntity entity)
    {
        return new MusicProviderDto(
            MusicProviderId: entity.MusicProviderId,
            Name: entity.Name,
            Image: entity.Image,
            Uri: entity.Uri,
            TotalArtists: entity.Artists?.Count ?? 0
        );
    }
}
