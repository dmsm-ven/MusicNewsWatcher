using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Core;

public class NewAlbumsFoundEventArgs
{
    public string Provider { get; init; }
    public ArtistDto Artist { get; init; }
    public AlbumDto[] NewAlbums { get; init; }
}
