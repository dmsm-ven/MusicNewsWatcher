﻿using MusicNewsWatcher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicNewsWatcher.TelegramBot;

public interface IMusicNewsMessageFormatter
{
    string NewAlbumsFoundMessage(string providerName, ArtistDto artist, IEnumerable<AlbumDto> albums);
}
