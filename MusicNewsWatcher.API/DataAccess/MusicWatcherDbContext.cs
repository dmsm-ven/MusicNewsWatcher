using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.Core.DataAccess.Entity;

namespace MusicNewsWatcher.API.DataAccess
{
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MusicProvider
            modelBuilder.Entity<MusicProviderEntity>(eb =>
            {
                eb.ToTable("musicproviders");
                eb.HasKey(x => x.MusicProviderId);
                eb.Property(x => x.MusicProviderId).HasColumnName("musicproviderid");
                eb.Property(x => x.Name).HasColumnName("name");
                eb.Property(x => x.Image).HasColumnName("image");
                eb.Property(x => x.Uri).HasColumnName("uri");

                eb.HasMany(x => x.Artists)
                  .WithOne(a => a.MusicProvider)
                  .HasForeignKey(a => a.MusicProviderId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // Artist
            modelBuilder.Entity<ArtistEntity>(eb =>
            {
                eb.ToTable("artists");
                eb.HasKey(x => x.ArtistId);
                eb.Property(x => x.ArtistId).HasColumnName("artistid");
                eb.Property(x => x.MusicProviderId).HasColumnName("musicproviderid");
                eb.Property(x => x.Name).HasColumnName("name");
                eb.Property(x => x.Uri).HasColumnName("uri");
                eb.Property(x => x.Image).HasColumnName("image");

                eb.HasMany(x => x.Albums)
                  .WithOne(a => a.Artist)
                  .HasForeignKey(a => a.ArtistId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // Album
            modelBuilder.Entity<AlbumEntity>(eb =>
            {
                eb.ToTable("albums");
                eb.HasKey(x => x.AlbumId);
                eb.Property(x => x.AlbumId).HasColumnName("albumid");
                eb.Property(x => x.ArtistId).HasColumnName("artistid");
                eb.Property(x => x.Title).HasColumnName("title");
                eb.Property(x => x.Created).HasColumnName("created");
                eb.Property(x => x.IsViewed).HasColumnName("isviewed");
                eb.Property(x => x.Image).HasColumnName("image");
                eb.Property(x => x.Uri).HasColumnName("uri");

                eb.HasMany(x => x.Tracks)
                  .WithOne(t => t.Album)
                  .HasForeignKey(t => t.AlbumId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // Track
            modelBuilder.Entity<TrackEntity>(eb =>
            {
                eb.ToTable("tracks");
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Id).HasColumnName("id");
                eb.Property(x => x.AlbumId).HasColumnName("albumid");
                eb.Property(x => x.Name).HasColumnName("name");
                eb.Property(x => x.DownloadUri).HasColumnName("downloaduri");
            });

            // Settings
            modelBuilder.Entity<SettingsEntity>(eb =>
            {
                eb.ToTable("settings");
                eb.HasKey(x => x.Name);
                eb.Property(x => x.Name).HasColumnName("name");
                eb.Property(x => x.Value).HasColumnName("value");
            });

            // SyncHost
            modelBuilder.Entity<SyncHostEntity>(eb =>
            {
                eb.ToTable("sync_host");
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Id).HasColumnName("id");
                eb.Property(x => x.Name).HasColumnName("name");
                eb.Property(x => x.RootFolderPath).HasColumnName("root_folder_path");
                eb.Property(x => x.Icon).HasColumnName("icon");
                eb.Property(x => x.LastUpdate).HasColumnName("last_update");

                eb.HasMany(x => x.SyncTracks)
                  .WithOne(t => t.Host)
                  .HasForeignKey(t => t.HostId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // SyncTrack (composite key)
            modelBuilder.Entity<SyncTrackEntity>(eb =>
            {
                eb.ToTable("sync_track");
                eb.HasKey(x => new { x.HostId, x.Path });
                eb.Property(x => x.HostId).HasColumnName("host_id");
                eb.Property(x => x.Path).HasColumnName("path");
                eb.Property(x => x.Hash).HasColumnName("hash");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
