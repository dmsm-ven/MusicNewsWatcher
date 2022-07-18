using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicNewsWatcher.DataAccess;

public class MusicProviderEntity
{
    [Key]
    public int MusicProviderId { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }

    public string Uri { get; set; }

    public List<ArtistEntity> Artists { get; } = new List<ArtistEntity>();
}
