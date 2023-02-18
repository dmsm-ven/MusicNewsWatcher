using System.Threading.Tasks;

namespace MusicNewsWatcher.Services;

public interface ISyncLibraryTracker
{
    Task CreateHost(SyncHostEntity host);
    Task<List<SyncHostEntity>> GetAllSyncHosts();
    Task<List<SyncTrackEntity>> GetAllTracksForHost(Guid hostId);
    Task AddTrackedItemsForHost(Guid hostId, string[] tracks);
    Task<string[]> GetTracksDiff(Guid id_left, Guid id_right);
}

public class SyncLibraryTracker : ISyncLibraryTracker
{
    private readonly IDbContextFactory<MusicWatcherDbContext> dbFactory;

    public SyncLibraryTracker(IDbContextFactory<MusicWatcherDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public async Task AddTrackedItemsForHost(Guid hostId, string[] tracks)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var items = tracks.Select(t => new SyncTrackEntity()
        {
            HostId = hostId,
            Path = t
        }).ToArray();


        var host = await db.SyncHosts.Include(h => h.SyncTracks).FirstOrDefaultAsync(h => h.Id == hostId);
        if (host != null)
        {
            host.SyncTracks.Clear();

            foreach (var item in items)
            {
                host.SyncTracks.Add(item);
            }

            host.LastUpdate = DateTimeOffset.Now;

            await db.SaveChangesAsync();
        }
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

        var list = await db.SyncHosts.AsNoTracking().ToListAsync();

        return list;
    }

    public async Task<List<SyncTrackEntity>> GetAllTracksForHost(Guid hostId)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var list = await db.SyncTracks
            .Where(st => st.HostId == hostId)
            .ToListAsync();

        return list;
    }

    public async Task<string[]> GetTracksDiff(Guid id_left, Guid id_right)
    {
        using var db = await dbFactory.CreateDbContextAsync();

        var leftHost = await db.SyncHosts.Include(h => h.SyncTracks).FirstOrDefaultAsync(h => h.Id == id_left);
        var rightHost = await db.SyncHosts.Include(h => h.SyncTracks).FirstOrDefaultAsync(h => h.Id == id_right);

        var filesLeft = leftHost.SyncTracks.AsEnumerable().Select(f => f.Path).ToList();
        var filesRight = rightHost.SyncTracks.AsEnumerable().Select(f => f.Path).ToList();

        if (filesLeft.Any() && filesRight.Any())
        {
            return filesRight.Except(filesLeft).ToArray();
        }

        return Enumerable.Empty<string>().ToArray();
    }
}