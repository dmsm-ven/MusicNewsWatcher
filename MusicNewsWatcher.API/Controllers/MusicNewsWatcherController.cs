﻿using Microsoft.AspNetCore.Mvc;
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
    private readonly ILogger<MusicNewsWatcherController> logger;

    public MusicNewsWatcherController(MusicWatcherDbContext db, MusicUpdateManager updateManager, ILogger<MusicNewsWatcherController> logger)
    {
        this.db = db;
        this.updateManager = updateManager;
        this.logger = logger;
    }

    [HttpGet]
    [Route("api/status")]
    public async Task<IActionResult> CheckStatus()
    {
        var isOk = await db.Database.CanConnectAsync();
        return isOk ? Ok() : Problem();
    }

    [HttpGet]
    [Route("api/providers")]
    public async Task<ActionResult<IEnumerable<MusicProviderDto>>> GetProviders()
    {
        // Количество артистов считается в SQL, а не загрузкой всех артистов в память
        var providers = await db.MusicProviders
            .AsNoTracking()
            .Select(p => new MusicProviderDto(p.MusicProviderId, p.Name, p.Image, p.Uri, p.Artists.Count))
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

        // Compute the latest album date in SQL. This avoids loading every artist's albums separately.
        var artists = await db.Artists
            .AsNoTracking()
            .Where(a => a.MusicProviderId == provider_id)
            .Select(a => new ArtistDto
            {
                ArtistId = a.ArtistId,
                MusicProviderId = a.MusicProviderId,
                Name = a.Name,
                Uri = a.Uri,
                Image = a.Image,
                LastAlbumCreated = a.Albums
                    .Select(album => (DateTime?)album.Created)
                    .Max()
            })
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
            .AsNoTracking()
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
    public async Task<ActionResult<IEnumerable<TrackDto>>> GetAlbumTracks([FromRoute] int album_id, [FromRoute] int provider_id)
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
        if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest();
        }

        bool isProviderExists = await db.MusicProviders.AnyAsync(a => a.MusicProviderId == dto.MusicProviderId);

        // Один и тот же исполнитель может отслеживаться на разных провайдерах, поэтому уникальность проверяется в рамках провайдера
        bool isArtistAlreadyExists = await db.Artists.AnyAsync(a => a.MusicProviderId == dto.MusicProviderId && a.Name == dto.Name);

        if (!isProviderExists || isArtistAlreadyExists)
        {
            return BadRequest();
        }

        var item = dto.ToEntity();
        db.Set<ArtistEntity>().Add(item);
        await db.SaveChangesAsync();

        // Первичный сбор альбомов выполняется в фоне. Ошибки логируем, иначе они бы остались незамеченными
        int providerId = dto.MusicProviderId;
        int artistId = item.ArtistId;
        _ = Task.Run(async () =>
        {
            try
            {
                await updateManager.CheckUpdatesForArtistAsync(providerId, artistId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка первичного получения альбомов артиста {artistId}", artistId);
            }
        });

        return Ok(item.ToDto());
    }

    [HttpPut]
    [Route("api/artists/{artist_id}")]
    public async Task<ActionResult<ArtistDto>> UpdateArtist(int artist_id, [FromBody] ArtistDto dto)
    {
        var artist = await db.Artists.FirstOrDefaultAsync(i => i.ArtistId == artist_id);

        if (artist is null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest();
        }

        // Обновляем поля существующей записи. Прежний вариант "удалить и добавить заново с тем же ключом"
        // мог привести к исключению трекера EF или каскадному удалению всех альбомов и треков артиста.
        artist.Name = dto.Name;
        artist.Uri = dto.Uri;
        artist.Image = dto.Image;

        await db.SaveChangesAsync();

        return Ok(artist.ToDto());
    }
}
