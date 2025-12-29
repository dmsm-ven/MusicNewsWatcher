using System;
using System.Collections.Generic;

namespace MusicNewsWatcher.Core.DataAccess.Entity
{
    public class SyncHostEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string RootFolderPath { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public DateTimeOffset? LastUpdate { get; set; }

        public ICollection<SyncTrackEntity> SyncTracks { get; set; } = new List<SyncTrackEntity>();
    }
}
