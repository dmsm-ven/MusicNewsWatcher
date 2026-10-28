namespace MusicNewsWatcher.API.Models;

public class AuthorizeMiddlewareOptions
{
    public string AccessToken { get; set; } = string.Empty;
    public bool IPFilteringEnabled { get; set; } = false;
    public bool AccessTokenRequired { get; set; } = true;
}
