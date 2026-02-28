using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.Controllers;

[ApiController]
public class DownloadHistoryController(MusicWatcherDbContext db, ILogger<DownloadHistoryController> logger) : ControllerBase
{
    private static readonly int MAX_HISTORY_ITEMS_LIMIT = 1000;

    [Route("api/download-history")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrackDownloadHistoryDto>>> GetHistory([FromQuery] int limit = 100)
    {

        var historyItems = await db.DownloadHistory
            .AsNoTracking()
            .OrderByDescending(i => i.Id)
            .Include(p => p.Track)
            .ThenInclude(p => p.Album)
            .ThenInclude(a => a.Artist)
            .Take((limit < MAX_HISTORY_ITEMS_LIMIT) ? limit : MAX_HISTORY_ITEMS_LIMIT)
            .ToArrayAsync();

        if (historyItems.Length == 0)
        {
            return NoContent();
        }

        var historyItemsDto = historyItems
            .Select(i => new TrackDownloadHistoryDto(
                ArtistName: i.Track?.Album?.Artist?.Name ?? "artist_name",
                AlbumName: i.Track?.Album?.Title ?? "album_name",
                TrackName: i.Track?.Name ?? "track_name",
                i.Started,
                i.Finished,
                i.FileSizeInBytes))
            .ToArray();

        return historyItemsDto.Length > 0 ? Ok(historyItemsDto) : NoContent();
    }

    [Route("api/download-history")]
    [HttpPost]
    public async Task<IActionResult> AddTrackDownloadHistory([FromBody] TrackDownloadHistoryRequest dto)
    {
        var trackItem = await db.Tracks.FirstOrDefaultAsync(t => t.Id == dto.TrackId);
        if (trackItem is null || dto.Started > dto.Finished)
        {
            return BadRequest();
        }

        var entity = dto.ToEntity();
        entity.Track = trackItem;
        db.DownloadHistory.Add(entity);

        await db.SaveChangesAsync();

        return Ok();
    }
}



