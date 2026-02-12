using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.DataAccess.MapperExtensions;
using MusicNewsWatcher.API.Services;
using MusicNewsWatcher.Core.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.Controllers;

//TODO: заменить длинные url контроллеров
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
    public async Task<ActionResult<IEnumerable<ArtistDto>>> GetProviderArtists(int provider_id)
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
    [Route("api/artists/{artist_id}/albums")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetArtistAlbums(int artist_id)
    {
        bool artistExists = await db.Artists.AnyAsync(a => a.ArtistId == artist_id);
        if (!artistExists)
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
    [Route("api/artists/{artist_id}")]
    public async Task<IActionResult> DeleteArtist(int artist_id)
    {
        ArtistEntity? artist = await db.Artists.FirstOrDefaultAsync(a => a.ArtistId == artist_id);
        if (artist is null)
        {
            return NotFound();
        }

        db.Artists.Remove(artist);
        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    [Route("api/artists/{artist_id}/refresh-albums")]
    public async Task<IActionResult> CheckArtistAlbums(int artist_id)
    {
        var artist = await db.Artists.FirstOrDefaultAsync(a => a.ArtistId == artist_id);
        if (artist is null)
        {
            return NotFound();
        }

        await updateManager.CheckUpdatesForArtistAsync(artist.MusicProviderId, artist_id);

        return Accepted();
    }

    [HttpGet]
    [Route("api/albums/{album_id}/provider/{provider_id}")]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumTracks([FromRoute] int album_id, [FromRoute] int provider_id)
    {
        var album = await db.Albums.FirstOrDefaultAsync(a => a.AlbumId == album_id);
        if (album is null)
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
    [Route("api/artists")]
    public async Task<ActionResult<ArtistDto>> CreateArtist([FromBody] CreateArtistDto dto)
    {
        bool isProviderExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == dto.MusicProviderId);
        bool isArtistAlreadyExists = await db.Artists.AnyAsync(a => a.Name == dto.Name);

        if (!isProviderExists || isArtistAlreadyExists || string.IsNullOrWhiteSpace(dto?.Name))
        {
            return BadRequest();
        }

        var item = dto.ToEntity();
        db.Set<ArtistEntity>().Add(item);
        await db.SaveChangesAsync();

        updateManager.CheckUpdatesForArtistAsync(dto.MusicProviderId, item.ArtistId);

        return item != null ? Ok(item) : BadRequest();
    }

    [HttpPut]
    [Route("api/artists/{artist_id}")]
    public async Task<ActionResult<ArtistDto>> UpdateArtist(int artist_id, [FromBody] ArtistDto dto)
    {
        var artist = await db.Artists.FirstOrDefaultAsync(i => i.ArtistId == artist_id);

        if (artist is null)
        {
            return BadRequest();
        }

        var item = dto.ToEntity();
        db.Set<ArtistEntity>().Remove(artist);
        db.Set<ArtistEntity>().Add(item);

        await db.SaveChangesAsync();

        return item != null ? Ok(item) : BadRequest();
    }
}
