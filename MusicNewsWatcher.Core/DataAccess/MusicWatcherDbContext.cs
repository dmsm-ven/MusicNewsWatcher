using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace MusicNewsWatcher.Core;

public class MusicWatcherDbContext : DbContext
{
    public DbSet<MusicProviderEntity> MusicProviders { get; set; }
    public DbSet<ArtistEntity> Artists { get; set; }
    public DbSet<AlbumEntity> Albums { get; set; }
    public DbSet<TrackEntity> Tracks { get; set; }
    public DbSet<SettingsEntity> Settings { get; set; }
    public DbSet<SyncHostEntity> SyncHosts { get; set; }
    public DbSet<SyncTrackEntity> SyncTracks { get; set; }

    public MusicWatcherDbContext(DbContextOptions<MusicWatcherDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
