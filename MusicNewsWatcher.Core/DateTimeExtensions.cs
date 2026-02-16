namespace MusicNewsWatcher.Core;

public static class DateTimeExtensions
{
    public static string ToRussianLocalTime(this DateTime dateTime)
    {
        DateTime convertedTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
        return convertedTime.ToString();
    }
}
