using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core.DataAccess.Entity;

[Table(name: "musicproviders")]
public class MusicProviderEntity
{
    [Key]
    [Column(name: "musicproviderid")]
    public int MusicProviderId { get; set; }

    [Column(name: "name")]
    public string Name { get; set; } = string.Empty;

    [Column(name: "image")]
    public string Image { get; set; } = string.Empty;

    [Column(name: "uri")]
    public string Uri { get; set; } = string.Empty;

    public List<ArtistEntity> Artists { get; } = new List<ArtistEntity>();
}
