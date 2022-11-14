﻿using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MusicNewsWatcher.Core;

public sealed class MusifyMusicProvider : MusicProviderBase
{
    const string HOST = "https://musify.club";
    const string AlbumsXPath = "//div[@id='divAlbumsList']/div";

    public MusifyMusicProvider() : base(2, "Musify") 
    {
        client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
        client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        client.DefaultRequestHeaders.Add("Pragma", "no-cache");
        client.DefaultRequestHeaders.Add("User-Agent", "insomnia/2022.6.0");
        client.DefaultRequestHeaders.Add("Referer", HOST);
    }

    public override async Task<AlbumEntity[]> GetAlbumsAsync(ArtistEntity artist)
    {
        var doc = await GetDocument(artist.Uri + "/releases");

        if (doc == null || doc.DocumentNode.SelectSingleNode(AlbumsXPath) == null)
        {
            return Enumerable.Empty<AlbumEntity>().ToArray();
        }
        
        var parsedAlbums = doc.DocumentNode
            .SelectNodes("//div[@id='divAlbumsList']/div")
            .Select(div => new
            {
                Title = div.SelectSingleNode("./a/img").GetAttributeValue("alt", string.Empty).Trim(),
                Image = div.SelectSingleNode("./a/img").GetAttributeValue("data-src", String.Empty).Trim(),
                Uri = HOST + div.SelectSingleNode("./a").GetAttributeValue("href", String.Empty).Trim(),
                DateTimeString = div.SelectSingleNode(".//i[contains(@class, 'zmdi-calendar')]").ParentNode.InnerText.Trim(),
                AlbumType = int.Parse(div.GetAttributeValue("data-type", "0"))

            })
            .Where(a => Enum.IsDefined(typeof(MusifyAlbumType), a.AlbumType))
            .ToArray();

        if (parsedAlbums.Length > 0)
        {
            AlbumEntity[] albums = new AlbumEntity[parsedAlbums.Length];

            for(int i = 0; i< parsedAlbums.Length; i++)
            {
                albums[i] = new AlbumEntity()
                {
                    ArtistId = artist.ArtistId,
                    Title = parsedAlbums[i].Title,
                    Image = parsedAlbums[i].Image,
                    Uri = parsedAlbums[i].Uri
                };

                if (DateTime.TryParseExact(parsedAlbums[i].DateTimeString, "dd.MM.yyyy", null, DateTimeStyles.None, out var created))
                {
                    albums[i].Created = created;
                }
            }

            return albums;
        }
        

        return Enumerable.Empty<AlbumEntity>().ToArray();
    }

    public override async Task<TrackEntity[]> GetTracksAsync(AlbumEntity album)
    {
        if (!string.IsNullOrWhiteSpace(album.Uri) && album.Uri.Contains(HOST))
        {
            var doc = await GetDocument(album.Uri);
            const string trackXPath = "//div[contains(@id, 'playerDiv')]";

            if (doc != null && doc.DocumentNode.SelectSingleNode(trackXPath) != null)
            {
                var tracks = doc.DocumentNode.SelectNodes(trackXPath)
                    .Select(div => new TrackEntity()
                    {
                        Name = div.SelectSingleNode(".//div[@class='playlist__heading']/a[last()]").InnerText?.Trim() ?? "<Ошибка>",
                        AlbumId = album.AlbumId,
                        DownloadUri = HOST + div.SelectSingleNode(".//a[@itemprop='audio' and @href]")?.GetAttributeValue("href", string.Empty)
                    }).ToArray();

                return tracks;
            }
        }
        return Enumerable.Empty<TrackEntity>().ToArray();
    }

    public override async Task<ArtistEntity[]> SerchArtist(string searchText)
    {
        string uri = $"{HOST}/search/suggestions?term={HttpUtility.UrlEncode(searchText)}";
        
        try
        {
            var response = await client.GetFromJsonAsync<MusifySearchResultDto[]>(uri);
            if (response.Length > 0)
            {
                var data = response
                    .Where(i => i.category == "Исполнители")
                    .Take(10)
                    .Select(i => new ArtistEntity()
                    {
                        Name = i.label,
                        Image = i.image,
                        Uri = HOST + i.url
                    })
                    .ToArray();

                return data;
            }
        }
        catch (Exception ex)
        {
            
        }


        return await base.SerchArtist(searchText);
    }

    enum MusifyAlbumType 
    {
        Album = 2,//Студийный альбом = 2,
        EP = 3,//EP = 3,
        Single = 4,//Сингл = 4,
        Split = 12,//Сплиты = 12,
        Soundtrack = 14//Саундтреки = 14,
    }
}
