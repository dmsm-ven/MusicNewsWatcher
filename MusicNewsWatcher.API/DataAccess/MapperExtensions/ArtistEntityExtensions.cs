using MusicNewsWatcher.API.DataAccess.Entity;
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

    public static ArtistEntity ToEntity(this CreateArtistDto dto)
    {
        return new ArtistEntity()
        {
            MusicProviderId = dto.MusicProviderId,
            Name = dto.Name,
            Uri = dto.Uri,
            Image = dto.Image
        };
    }
    public static ArtistEntity ToEntity(this ArtistDto dto)
    {
        return new ArtistEntity()
        {
            MusicProviderId = dto.MusicProviderId,
            ArtistId = dto.ArtistId,
            Name = dto.Name,
            Uri = dto.Uri,
            Image = dto.Image
        };
    }
}