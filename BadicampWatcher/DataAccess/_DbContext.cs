using Microsoft.EntityFrameworkCore;
using System;

namespace BandcampWatcher.DataAccess;

public class BandcampWatcherDbContext : DbContext
{
    public DbSet<Artist> Artists { get; set;}
    public DbSet<Album> Albums { get; set;}

    public string ConnectionString { get; } = $"Data Source=settings.db";

    public BandcampWatcherDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    { 
       options.UseSqlite(ConnectionString);
    }
}
