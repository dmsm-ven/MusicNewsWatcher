using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandcampWatcher.DataAccess;

public class Artist
{
    public int ArtistId { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public string Image { get; set; }
    public List<Album> Albums { get; } = new List<Album>();
}
