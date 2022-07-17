using BandcampWatcher.ViewModels;
using System.Collections.Generic;

namespace BandcampWatcher.Models;

public class MusicProviderModel : ViewModelBase
{
    public int MusicProviderId { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }

    public List<ArtistViewModel> Artists { get; set; } = new List<ArtistViewModel>();
}
