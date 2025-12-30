using MusicNewsWatcher.API.DataAccess.Entity;

namespace MusicNewsWatcher.API;

public abstract class MusicProviderBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    public abstract Task<AlbumEntity[]> GetAlbumsAsync(ArtistEntity artist);
    public abstract Task<TrackEntity[]> GetTracksAsync(AlbumEntity album);

    //Переопределяем метод если провайдера позволяет делать поиск по сайту, иначе просто возвращаем пустой массив
    public virtual Task<ArtistEntity[]> SerchArtist(string searchText)
    {
        return Task.FromResult(Enumerable.Empty<ArtistEntity>().ToArray());
    }
    protected MusicProviderBase(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
