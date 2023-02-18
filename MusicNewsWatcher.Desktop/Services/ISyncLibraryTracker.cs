using System.Threading.Tasks;

namespace MusicNewsWatcher.Services;

public interface ISyncLibraryTracker
{
    Task CreateHost(SyncHostEntity host);
    Task<List<SyncHostEntity>> GetAllSyncHosts();
}

public class SyncLibraryTracker : ISyncLibraryTracker
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;

    public SyncLibraryTracker(IDbContextFactory<MusicWatcherDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public async Task CreateHost(SyncHostEntity host)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        if (!string.IsNullOrWhiteSpace(host.Name) && db.SyncHosts.FirstOrDefault(sh => sh.Name == host.Name) == null)
        {
            db.SyncHosts.Add(host);

            await db.SaveChangesAsync();
        }
    }

    public async Task<List<SyncHostEntity>> GetAllSyncHosts()
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var list = await db.SyncHosts.ToListAsync();

        return list;
    }
}