using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.Core.DataAccess.Entity;

public static class TrackEntityExtensions
{
    public static TrackDto ToDto(this TrackEntity entity)
    {
        return new TrackDto(
            Id: entity.Id,
            AlbumId: entity.AlbumId,
            Name: entity.Name,
            DownloadUri: entity.DownloadUri
        );
    }

    public static TrackEntity ToEntity(this TrackDto dto)
    {
        return new TrackEntity()
        {
            AlbumId = dto.AlbumId,
            DownloadUri = dto.DownloadUri,
            Name = dto.Name,
            Id = dto.Id,
        };
    }

}