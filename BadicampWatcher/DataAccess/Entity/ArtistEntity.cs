using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.DataAccess;

public class ArtistEntity
{
    [Key]
    public int ArtistId { get; set; }

    public int MusicProviderId { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public string Image { get; set; }

    public List<AlbumEntity> Albums { get; set; } = new List<AlbumEntity>();
    public MusicProviderEntity MusicProvider { get; set; }
}
