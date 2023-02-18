using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core;

[Table(name: "sync_host")]
public class SyncHostEntity
{
    [Key]
    [Column(name: "id")]
    public Guid Id { get; set; }

    [Column(name: "name")]
    public string Name { get; set; }

    [Column(name: "root_folder_path")]
    public string RootFolderPath { get; set; } = string.Empty;

    [Column(name: "icon")]
    public string Icon { get; set; }

    [Column(name: "last_update")]
    public DateTimeOffset? LastUpdate { get; set; }

    public ICollection<SyncTrackEntity> SyncTracks { get; set; } = new List<SyncTrackEntity>();
}
