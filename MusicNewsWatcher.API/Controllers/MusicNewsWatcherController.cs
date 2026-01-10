using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.Controllers;

[ApiController]
public class MusicNewsWatcherController : ControllerBase
{
    private readonly MusicWatcherDbContext db;
    private readonly MusicUpdateManager updateManager;

    public MusicNewsWatcherController(MusicWatcherDbContext db, MusicUpdateManager updateManager)
    {
        this.db = db;
        this.updateManager = updateManager;
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

    [HttpDelete]
    [Route("api/providers/{provider_id}/artists/{artist_id}")]
    public async Task<IActionResult> DeleteArtist([FromRoute] int provider_id, [FromRoute] int artist_id)
    {
        bool providerExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        ArtistEntity? artist = await db.Artists.FirstOrDefaultAsync(a => a.ArtistId == artist_id);
        if (!providerExists || artist == null)
        {
            return NotFound();
        }

        db.Artists.Remove(artist);
        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    [Route("api/providers/{provider_id}/artists/{artist_id}/albums/refresh")]
    public async Task<IActionResult> CheckArtistAlbums([FromRoute] int provider_id, [FromRoute] int artist_id)
    {
        bool providerExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        bool artistExists = await db.Artists.AnyAsync(a => a.ArtistId == artist_id);
        if (!providerExists || !artistExists)
        {
            return NotFound();
        }

        await updateManager.CheckUpdatesForArtistAsync(provider_id, artist_id);

        return Accepted();
    }

    [HttpGet]
    [Route("api/providers/{provider_id}/artists/{artist_id}/albums/{album_id}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumTracks([FromRoute] int provider_id,
        [FromRoute] int artist_id,
        [FromRoute] int album_id)
    {
        bool providerExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        bool artistExists = await db.Artists.AnyAsync(a => a.ArtistId == artist_id);
        bool albumExists = await db.Albums.AnyAsync(a => a.AlbumId == album_id);
        if (!providerExists || !artistExists || !albumExists)
        {
            return NotFound();
        }

        var tracks = await db.Tracks
            .Where(t => t.AlbumId == album_id)
            .Select(track => track.ToDto())
            .ToListAsync();

        if (tracks.Count == 0)
        {
            await updateManager.CheckUpdatesForAlbumAsync(provider_id, album_id);
            tracks = await db.Tracks
                .Where(t => t.AlbumId == album_id)
                .Select(track => track.ToDto())
                .ToListAsync();
        }

        return tracks.Any() ? Ok(tracks) : NoContent();
    }

    [HttpPost]
    [Route("api/providers/{provider_id}/artists")]
    public async Task<ActionResult<ArtistDto>> CreateArtist([FromRoute] int provider_id, [FromBody] CreateArtistDto dto)
    {
        bool isProviderExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == provider_id);
        bool isArtistAlreadyExists = await db.Artists.AnyAsync(a => a.Name == dto.Name);

        if (!isProviderExists || isArtistAlreadyExists || string.IsNullOrWhiteSpace(dto?.Name))
        {
            return BadRequest();
        }

        var item = dto.ToEntity();
        db.Set<ArtistEntity>().Add(item);
        await db.SaveChangesAsync();

        updateManager.CheckUpdatesForArtistAsync(provider_id, item.ArtistId);

        return item != null ? Ok(item) : BadRequest();
    }
}
