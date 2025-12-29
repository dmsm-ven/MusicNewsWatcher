using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.DataAccess.MapperExtensions;

public static class ArtistEntityExtensions
{
    public static ArtistDto ToDto(this ArtistEntity entity)
    {
        return new ArtistDto(
            ArtistId: entity.ArtistId,
            MusicProviderId: entity.MusicProviderId,
            Name: entity.Name,
            Uri: entity.Uri,
            Image: entity.Image
        );
    }
}