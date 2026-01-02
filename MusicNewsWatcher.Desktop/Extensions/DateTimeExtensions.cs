namespace MusicNewsWatcher.Desktop.Extensions;

public static class DateTimeExtensions
{
    public static string ToLocalRuDateAndTime(this DateTimeOffset dt)
    {
        return dt
            .ToLocalTime()
            .ToString("dd.MM.yyyy HH:mm");
    }
}
