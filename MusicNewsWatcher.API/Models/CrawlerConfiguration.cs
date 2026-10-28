namespace MusicNewsWatcher.API.Models;

public class CrawlerConfiguration
{
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(60);

    /// <summary>
    /// Базовая пауза между запросами к сайтам-источникам при переобходе (к ней добавляется случайная добавка до 50%),
    /// чтобы не получить блокировку за слишком частые запросы
    /// </summary>
    public TimeSpan RequestDelay { get; set; } = TimeSpan.FromSeconds(1.5);
}
