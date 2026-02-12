using MusicNewsWatcher.API.DataAccess.Entity;
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

    public static AlbumEntity ToEntity(this AlbumDto dto)
    {
        return new()
        {
            ArtistId = dto.ArtistId,
            Title = dto.Title,
            Image = dto.Image,
            Uri = dto.Uri,
            Created = dto.Created
        };
    }
}