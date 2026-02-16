namespace MusicNewsWatcher.TelegramBot;

internal class CommandDescriptorEntry
{
    public string Route { get; init; } = "/route_path";
    public string Description { get; init; } = string.Empty;
    public Func<Task<string>>? Callback { get; init; } = null;
}