using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicNewsWatcher.Core;

[Table(name: "musicproviders")]
public class MusicProviderEntity
{
    [Key]
    [Column(name: "musicproviderid")]
    public int MusicProviderId { get; set; }

    [Column(name: "name")]
    public string Name { get; set; }

    [Column(name: "image")]
    public string Image { get; set; }

    [Column(name: "uri")]
    public string Uri { get; set; }

    public List<ArtistEntity> Artists { get; } = new List<ArtistEntity>();
}
