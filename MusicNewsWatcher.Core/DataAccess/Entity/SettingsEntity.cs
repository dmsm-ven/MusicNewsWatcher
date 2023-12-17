using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core.DataAccess.Entity;

[Table(name: "settings")]
public class SettingsEntity
{
    [Key]
    [Column(name: "name")]
    public string Name { get; set; } = string.Empty;

    [Column(name: "value")]
    public string Value { get; set; } = string.Empty;
}
