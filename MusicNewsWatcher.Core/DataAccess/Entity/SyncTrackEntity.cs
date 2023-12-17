using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core.DataAccess.Entity;

[PrimaryKey(nameof(HostId), nameof(Path))]
[Table(name: "sync_track")]
public class SyncTrackEntity
{
    [Column(name: "host_id")]
    public Guid HostId { get; set; }

    [Column(name: "path")]
    public string Path { get; set; } = string.Empty;

    [Column(name: "hash")]
    public string Hash { get; set; } = string.Empty;

    [ForeignKey(nameof(HostId))]
    public SyncHostEntity? Host { get; set; }
}