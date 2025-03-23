using MusicNewsWatcher.Core.DataAccess.Entity;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Desktop.Services;

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
    private readonly MusicWatcherDbContext dbContext;

    public SyncLibraryTracker(MusicWatcherDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddTrackedItemsForHost(Guid hostId, string[] tracks)
    {
        var items = tracks.Select(t => new SyncTrackEntity()
        {
            HostId = hostId,
            Path = t
        }).ToArray();


        var host = await dbContext.SyncHosts.Include(h => h.SyncTracks).FirstOrDefaultAsync(h => h.Id == hostId);
        if (host != null)
        {
            host.SyncTracks.Clear();

            foreach (var item in items)
            {
                host.SyncTracks.Add(item);
            }

            host.LastUpdate = DateTimeOffset.Now;

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task CreateHost(SyncHostEntity host)
    {
        if (!string.IsNullOrWhiteSpace(host.Name) && dbContext.SyncHosts.FirstOrDefault(sh => sh.Name == host.Name) == null)
        {
            dbContext.SyncHosts.Add(host);

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<SyncHostEntity>> GetAllSyncHosts()
    {
        var list = await dbContext.SyncHosts.AsNoTracking().ToListAsync();

        return list;
    }

    public async Task<List<SyncTrackEntity>> GetAllTracksForHost(Guid hostId)
    {
        var list = await dbContext.SyncTracks
            .Where(st => st.HostId == hostId)
            .ToListAsync();

        return list;
    }

    public async Task<string[]> GetTracksDiff(Guid id_left, Guid id_right)
    {
        var leftHost = await dbContext.SyncHosts.Include(h => h.SyncTracks).FirstOrDefaultAsync(h => h.Id == id_left);
        var rightHost = await dbContext.SyncHosts.Include(h => h.SyncTracks).FirstOrDefaultAsync(h => h.Id == id_right);

        var filesLeft = leftHost!.SyncTracks.AsEnumerable().Select(f => f.Path).ToList()!;
        var filesRight = rightHost!.SyncTracks.AsEnumerable().Select(f => f.Path).ToList();

        if (filesLeft.Any() && filesRight.Any())
        {
            return filesRight.Except(filesLeft).ToArray();
        }

        return Enumerable.Empty<string>().ToArray();
    }
}