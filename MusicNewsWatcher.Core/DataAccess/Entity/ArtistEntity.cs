using System.Collections.Generic;

namespace MusicNewsWatcher.Core.DataAccess.Entity
{
    public class ArtistEntity
    {
        public int ArtistId { get; set; }

        public int MusicProviderId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Uri { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public List<AlbumEntity> Albums { get; set; } = new List<AlbumEntity>();

        public MusicProviderEntity? MusicProvider { get; set; }
    }
}
