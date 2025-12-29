using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.DataAccess.MapperExtensions;

public static class AlbumEntityExtensions
{
    public static AlbumDto ToDto(this AlbumEntity entity)
    {
        return new AlbumDto(
            AlbumId: entity.AlbumId,
            ArtistId: entity.ArtistId,
            Title: entity.Title,
            Created: entity.Created,
            IsViewed: entity.IsViewed,
            Image: entity.Image,
            Uri: entity.Uri
        );
    }
}