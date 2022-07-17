using BandcampWatcher.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BandcampWatcher.Models;

public class MusifyMusicProvider : MusicProviderBase
{
    public MusifyMusicProvider() : base(2, "Musify") { }

    public override Task<List<AlbumEntity>> GetAlbums(ArtistEntity artist)
    {
        throw new NotImplementedException();
    }
}
