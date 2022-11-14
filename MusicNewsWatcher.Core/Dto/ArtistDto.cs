namespace MusicNewsWatcher.Core;

public record ArtistDto(string name, string uri);

public record MusifySearchResultDto
{
    public string id { get; init; }
    public string label { get; init; }
    public string value { get; init; }
    public string category { get; init; }
    public string image { get; init; }
    public string url { get; init; }
}
