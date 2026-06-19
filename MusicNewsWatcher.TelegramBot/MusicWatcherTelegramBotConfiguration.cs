namespace MusicNewsWatcher.TelegramBot;

public class MusicWatcherTelegramBotConfiguration
{
    public string ApiKey { get; set; } = string.Empty;
    public long ClientId { get; set; }
    public MusicWatcherTelegramBotProxy ProxySettings { get; set; } = new();
}

public class MusicWatcherTelegramBotProxy
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 443;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}