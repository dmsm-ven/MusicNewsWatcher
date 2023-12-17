using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core.DataAccess.Entity;

[Table(name: "sync_host")]
public class SyncHostEntity
{
    [Key]
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "name")]
    public string Name { get; set; } = string.Empty;

    [Column(name: "root_folder_path")]
    public string RootFolderPath { get; set; } = string.Empty;

    [Column(name: "icon")]
    public string Icon { get; set; } = string.Empty;

    [Column(name: "last_update")]
    public DateTimeOffset? LastUpdate { get; set; }

    public ICollection<SyncTrackEntity> SyncTracks { get; set; } = new List<SyncTrackEntity>();
}
