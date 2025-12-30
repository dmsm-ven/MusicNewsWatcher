using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.Controllers;

[ApiController]
public class MusicNewsWatcherController : ControllerBase
{
    private readonly MusicWatcherDbContext db;

    public MusicNewsWatcherController(MusicWatcherDbContext db)
    {
        this.db = db;
    }
    [HttpGet]
    [Route("api/providers")]
    public async Task<ActionResult<IEnumerable<MusicProviderDto>>> GetProviders()
    {
        var providers = await db.MusicProviders
            .Include(p => p.Artists)
            .Select(p => p.ToDto())
            .ToListAsync();

        return providers.Any() ? Ok(providers) : NoContent();
    }

    [HttpGet]
    [Route("api/providers/{provider_id}/artists")]
    public async Task<ActionResult<IEnumerable<ArtistDto>>> GetProviderArtists([FromRoute] int provider_id)
    {
        bool providerExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        if (!providerExists)
        {
            return NotFound();
        }

        var artists = await db.Artists
            .Include(a => a.Albums)
            .Where(a => a.MusicProviderId == provider_id)
            .Select(p => p.ToDto())
            .ToListAsync();

        return artists.Any() ? Ok(artists) : NoContent();
    }

    [HttpGet]
    [Route("api/providers/{provider_id}/artists/{artist_id}/albums")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetArtistAlbums([FromRoute] int provider_id, [FromRoute] int artist_id)
    {
        bool providerExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        bool artistExists = await db.Artists.AnyAsync(a => a.ArtistId == artist_id);
        if (!providerExists || !artistExists)
        {
            return NotFound();
        }

        var albums = await db.Albums
            .Where(a => a.ArtistId == artist_id)
            .Select(a => a.ToDto())
            .ToListAsync();

        return albums.Any() ? Ok(albums) : NoContent();
    }

    [HttpGet]
    [Route("api/providers/{provider_id}/artists/{artist_id}/albums/{album_id}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumTracks([FromRoute] int provider_id, [FromRoute] int artist_id, [FromRoute] int album_id)
    {
        bool providerExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        bool artistExists = await db.Artists.AnyAsync(a => a.ArtistId == artist_id);
        bool albumExists = await db.Albums.AnyAsync(a => a.AlbumId == album_id);
        if (!providerExists || !artistExists || !albumExists)
        {
            return NotFound();
        }

        var albums = await db.Albums
            .Where(a => a.ArtistId == artist_id)
            .Select(a => a.ToDto())
            .ToListAsync();

        return albums.Any() ? Ok(albums) : NoContent();
    }
}
