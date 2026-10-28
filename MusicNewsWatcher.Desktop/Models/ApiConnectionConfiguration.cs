namespace MusicNewsWatcher.Desktop.Models;

public record ApiConnectionConfiguration(string Host, string AccessToken);

public record HttpClientParserConfiguration(string UserAgent);