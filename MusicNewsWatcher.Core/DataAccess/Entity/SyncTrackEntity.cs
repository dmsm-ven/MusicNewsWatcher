using System;

namespace MusicNewsWatcher.Core.DataAccess.Entity
{
    public class SyncTrackEntity
    {
        public Guid HostId { get; set; }

        public string Path { get; set; } = string.Empty;

        public string Hash { get; set; } = string.Empty;

        public SyncHostEntity? Host { get; set; }
    }
}