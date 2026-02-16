using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API.DataAccess.Entity;

namespace MusicNewsWatcher.API.DataAccess
{
    public class MusicWatcherDbContext : DbContext
    {
        public DbSet<MusicProviderEntity> MusicProviders { get; set; }
        public DbSet<ArtistEntity> Artists { get; set; }
        public DbSet<AlbumEntity> Albums { get; set; }
        public DbSet<TrackEntity> Tracks { get; set; }
        public DbSet<SettingsEntity> Settings { get; set; }
        public DbSet<TrackDownloadHistoryEntity> DownloadHistory { get; set; }

        public MusicWatcherDbContext(DbContextOptions<MusicWatcherDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
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

            // DownloadHistory
            modelBuilder.Entity<TrackDownloadHistoryEntity>(eb =>
            {
                eb.ToTable("download_hist");
                eb.HasKey(x => x.Id);
                eb.Property(x => x.Id).HasColumnName("id");
                eb.Property(x => x.TrackId).HasColumnName("track_id");
                eb.Property(x => x.Started).HasColumnName("started");
                eb.Property(x => x.Finished).HasColumnName("finished");
                eb.Property(x => x.FileSizeInBytes).HasColumnName("file_size_in_bytes");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
