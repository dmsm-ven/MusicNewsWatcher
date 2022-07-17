using Microsoft.EntityFrameworkCore;
using System;

namespace BandcampWatcher.DataAccess;

public class MusicWatcherDbContext : DbContext
{
    public DbSet<MusicProviderEntity> MusicProviders { get; set; }
    public DbSet<ArtistEntity> Artists { get; set;}
    public DbSet<AlbumEntity> Albums { get; set;}

    public MusicWatcherDbContext(DbContextOptions<MusicWatcherDbContext> options) : base(options)
    {
        //Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=settings.db");
    }

}
