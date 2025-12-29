namespace MusicNewsWatcher.Core.Models.Musify;

public record MusifySearchResultDto
{
    public string Id { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Image { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
}
