using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace MusicNewsWatcher.DataAccess;

public class MusicWatcherDbContext : DbContext
{
    public DbSet<MusicProviderEntity> MusicProviders { get; set; }
    public DbSet<ArtistEntity> Artists { get; set;}
    public DbSet<AlbumEntity> Albums { get; set;}
    public DbSet<TrackEntity> Tracks { get; set;}
    public DbSet<SettingsEntity> Settings { get; set;}

    public MusicWatcherDbContext(DbContextOptions<MusicWatcherDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=settings.db");
        options.LogTo(Console.WriteLine);
    }

}

public class MusicWatcherDbContextFactory : IDesignTimeDbContextFactory<MusicWatcherDbContext>
{
    public MusicWatcherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MusicWatcherDbContext>();
        optionsBuilder.UseSqlite("Data Source=settings.db");

        return new MusicWatcherDbContext(optionsBuilder.Options);
    }
}
