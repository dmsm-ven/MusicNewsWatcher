using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Services;

public interface IMusicNewsMessageFormatter
{
    string NewAlbumsFoundMessage(string providerName, ValueTuple<string, string> artistWithUri, IEnumerable<ValueTuple<string, string>> albumNamesWithUrl);
}
