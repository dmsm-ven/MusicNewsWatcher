﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicNewsWatcher.DataAccess;

public class AlbumEntity
{
    [Key]
    public int AlbumId { get; set; }

    public int ArtistId { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; }
    public bool IsViewed { get; set; }
    public string? Image { get; set; }
    public string Uri { get; set; }

    public ArtistEntity Artist { get; set; }
    public List<TrackEntity> Tracks { get; set; } = new List<TrackEntity>();
}
