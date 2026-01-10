using HtmlAgilityPack;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.Core.Models.Dtos;

namespace MusicNewsWatcher.API.MusicProviders.Base;

public abstract class MusicProviderBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    public virtual string AlbumsUriPath(ArtistEntity artist) => throw new NotSupportedException();
    public abstract Task<AlbumDto[]> GetAlbumsAsync(ArtistEntity artist, HtmlDocument doc);
    public abstract Task<TrackDto[]> GetTracksAsync(AlbumEntity album, HtmlDocument doc);

    //Переопределяем метод если провайдера позволяет делать поиск по сайту, иначе просто возвращаем пустой массив
    public virtual Task<AlbumDto[]> SerchArtist(string searchText)
    {
        return Task.FromResult(Array.Empty<AlbumDto>());
    }
}
