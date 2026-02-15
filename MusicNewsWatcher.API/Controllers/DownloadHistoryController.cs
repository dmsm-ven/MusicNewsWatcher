using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.Controllers;

[ApiController]
public class DownloadHistoryController(MusicWatcherDbContext db) : ControllerBase
{
    private static readonly int MAX_HISTORY_ITEMS_LIMIT = 1000;

    [Route("api/download-history")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrackDownloadHistoryDto>>> GetHistory([FromQuery] int limit = 100)
    {
        var items = (await db.DownloadHistory
            .AsNoTracking()
            .Include(a => a.Track)
            .OrderByDescending(i => i.Id)
            .Take((limit < MAX_HISTORY_ITEMS_LIMIT) ? limit : MAX_HISTORY_ITEMS_LIMIT)
            .ToArrayAsync())
            .Select(i => i.ToDto())
            .ToArray() ?? Array.Empty<TrackDownloadHistoryDto>();

        return items.Length > 0 ? Ok(items) : NoContent();
    }

    [Route("api/download-history")]
    [HttpPost]
    public async Task<IActionResult> AddTrackDownloadHistory([FromBody] TrackDownloadHistoryRequest dto)
    {
        var trackItem = await db.Tracks.FirstOrDefaultAsync(t => t.Id == dto.trackId);
        if (trackItem is null || dto.Started > dto.Finished)
        {
            return BadRequest();
        }

        await db.DownloadHistory.AddAsync(dto.ToEntity());

        return Ok();
    }
}



