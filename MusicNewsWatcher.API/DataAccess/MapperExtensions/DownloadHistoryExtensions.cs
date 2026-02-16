using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.DataAccess.MapperExtensions;

internal static class DownloadHistoryExtensions
{
    public static TrackDownloadHistoryEntity ToEntity(this TrackDownloadHistoryRequest dto)
    {
        return new TrackDownloadHistoryEntity()
        {
            FileSizeInBytes = dto.FileSizeInBytes,
            Finished = dto.Finished,
            Started = dto.Started,
            TrackId = dto.TrackId,
        };
    }
}



