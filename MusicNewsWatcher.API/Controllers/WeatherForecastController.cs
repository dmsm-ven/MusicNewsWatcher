using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewsWatcher.API.Controllers;

[ApiController]
[Authorize]
public class MusicNewsWatcherController : ControllerBase
{
    private readonly MusicWatcherDbContext db;

    public MusicNewsWatcherController(MusicWatcherDbContext db)
    {
        this.db = db;
    }
    [HttpGet]
    [Route("api/providers")]
    public async Task<ActionResult<IEnumerable<MusicProviderEntity>>> GetProviders()
    {
        var providers = await db.MusicProviders.ToListAsync();
        return providers.Any() ? Ok(providers) : NoContent();
    }
}
