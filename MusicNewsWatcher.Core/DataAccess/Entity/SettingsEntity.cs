using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core;

[Table(name: "settings")]
public class SettingsEntity
{
    [Key]
    [Column(name: "name")]
    public string Name { get; set; }

    [Column(name: "value")]
    public string Value { get; set; } = string.Empty;
}
