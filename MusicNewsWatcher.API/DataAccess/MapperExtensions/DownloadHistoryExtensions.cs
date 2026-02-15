using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.DataAccess.MapperExtensions;

internal static class DownloadHistoryExtensions
{
    public static TrackDownloadHistoryDto ToDto(this TrackDownloadHistoryEntity entity)
    {
        return new(entity.Track.ToDto(), entity.Started, entity.Finished, entity.FileSizeInKb);
    }
    public static TrackDownloadHistoryEntity ToEntity(this TrackDownloadHistoryRequest dto)
    {
        return new TrackDownloadHistoryEntity()
        {
            FileSizeInKb = dto.FileSizeInKb,
            Finished = dto.Finished,
            Started = dto.Started,
            TrackId = dto.trackId,
        };
    }
}



