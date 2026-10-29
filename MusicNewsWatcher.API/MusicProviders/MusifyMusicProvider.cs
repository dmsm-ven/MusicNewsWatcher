using HtmlAgilityPack;
using MusicNewsWatcher.API.DataAccess.Entity;
using MusicNewsWatcher.API.MusicProviders.Base;
using MusicNewsWatcher.Core.Models.Dtos;
using System.Text.RegularExpressions;

namespace MusicNewsWatcher.API.MusicProviders;

public sealed class MusifyMusicProvider : MusicProviderBase
{
    private const string HOST = "https://musify.club";
    private const string AlbumsXPath = "//div[@id='divAlbumsList']/article";
    public override string AlbumsUriPath(ArtistEntity artist)
    {
        return $"{artist.Uri}/discography";
    }
    public MusifyMusicProvider()
    {
        this.Id = 2;
        this.Name = "Musify";
    }
    public override Task<AlbumDto[]> GetAlbumsAsync(ArtistEntity artist, HtmlDocument doc)
    {
        // SelectNodes возвращает null, если ничего не найдено
        var albumNodes = doc?.DocumentNode?.SelectNodes(AlbumsXPath);
        if (albumNodes == null)
        {
            return Task.FromResult(Array.Empty<AlbumDto>());
        }

        var albums = new List<AlbumDto>();

        foreach (var article in albumNodes)
        {
            // Любое отклонение разметки одного альбома пропускает только этот альбом, а не весь разбор артиста
            var href = article?.SelectSingleNode("./a")?.GetAttributeValue("href", string.Empty).Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(href))
            {
                continue;
            }

            if (!int.TryParse(article?.GetAttributeValue("data-type", "0"), out int albumType) ||
                !Enum.IsDefined(typeof(MusifyAlbumType), albumType))
            {
                continue;
            }

            string title = article.SelectSingleNode(".//h3[@class='release-thumbnail__title']/a")?.InnerText?.Trim() ?? "<Ошибка>";
            string image = article?.SelectSingleNode("./a[@class='release-thumbnail__cover']/picture/img")?.GetAttributeValue("src", string.Empty).Trim() ?? "";
            string uri = href.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? href : HOST + href;

            string releaseYearString = article?.SelectSingleNode(".//a[@class='release-thumbnail__year tabular-nums']")?.GetAttributeValue("title", "") ?? "";
            const int yearOfReleaseMIN = 1900;
            const int yearOfReleaseMAX = 2100;
            if (!int.TryParse(releaseYearString, out var yearOfRelease) || yearOfRelease <= yearOfReleaseMIN || yearOfRelease >= yearOfReleaseMAX)
            {
                yearOfRelease = yearOfReleaseMIN;
            }

            albums.Add(new AlbumDto(0, artist.ArtistId, title, new DateTime(yearOfRelease, 1, 1), false, image, uri));
        }

        return Task.FromResult(albums.ToArray());
    }
    public override Task<TrackDto[]> GetTracksAsync(AlbumEntity album, HtmlDocument doc)
    {
        if (string.IsNullOrWhiteSpace(album.Uri) || !album.Uri.Contains(HOST))
        {
            return Task.FromResult(Array.Empty<TrackDto>());
        }

        var trackNodes = doc?.DocumentNode?.SelectNodes("//div[contains(@id, 'playerDiv')]//a[@itemprop='url']");
        if (trackNodes == null)
        {
            return Task.FromResult(Array.Empty<TrackDto>());
        }

        var tracks = trackNodes
            .Select(a =>
            {
                Match match = Regex.Match(
                    a.GetAttributeValue("href", string.Empty),
                    @"/track/(?<trackName>.+)-(?<trackId>\d+)$");

                var trackUrl = match.Success ?
                    $"https://musify.club/track/dl/{match.Groups["trackId"].Value}/{match.Groups["trackName"].Value}.mp3"
                    : "";

                return new TrackDto(Id: 0,
                    AlbumId: album.AlbumId,
                    Name: a.InnerText?.Trim() ?? "<Ошибка парсинга названия трека>",
                    DownloadUri: trackUrl);
            })
            .ToArray();

        return Task.FromResult(tracks);
    }
    public override Task<AlbumDto[]> SerchArtist(string searchText)
    {
        return base.SerchArtist(searchText);
        /*
        string uri = $"{HOST}/search/suggestions?term={HttpUtility.UrlEncode(searchText)}";

        try
        {
            var response = await this.httpClient.GetFromJsonAsync<MusifySearchResultDto[]>(uri);
            if (response != null && response.Length > 0)
            {
                var data = response
                    .Where(i => i.Category == "Исполнители")
                    .Take(10)
                    .Select(i => new ArtistEntity()
                    {
                        Name = i.Label,
                        Image = i.Image,
                        Uri = HOST + i.Uri
                    })
                    .ToArray();

                return data;
            }
        }
        catch
        {
            //ignore search errors
        }  
        */
    }
    private enum MusifyAlbumType
    {
        Album = 2,//Студийный альбом = 2,
        EP = 3,//EP = 3,
        Single = 4,//Сингл = 4,
        Split = 12,//Сплиты = 12,
        Soundtrack = 14//Саундтреки = 14,
    }
}
