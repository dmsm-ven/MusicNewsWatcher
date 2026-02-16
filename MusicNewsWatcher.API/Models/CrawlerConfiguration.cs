namespace MusicNewsWatcher.API.Models;

public class CrawlerConfiguration
{
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(60);
}

