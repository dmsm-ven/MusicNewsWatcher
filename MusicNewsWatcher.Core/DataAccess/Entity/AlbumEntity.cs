using System;
using System.Collections.Generic;

namespace MusicNewsWatcher.Core.DataAccess.Entity
{
    public class AlbumEntity
    {
        public int AlbumId { get; set; }

        public int ArtistId { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsViewed { get; set; }

        public string? Image { get; set; }

        public string Uri { get; set; } = string.Empty;

        public ArtistEntity? Artist { get; set; }
        public List<TrackEntity> Tracks { get; set; } = new();
    }
}
