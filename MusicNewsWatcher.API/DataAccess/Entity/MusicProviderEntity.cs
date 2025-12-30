namespace MusicNewsWatcher.API.DataAccess.Entity;

public class MusicProviderEntity
{
    public int MusicProviderId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public string Uri { get; set; } = string.Empty;

    public List<ArtistEntity> Artists { get; } = new List<ArtistEntity>();
}
