
using Microsoft.Extensions.Options;

namespace MusicNewsWatcher.API.Controllers;

public class AuthorizeMiddlewareOptions
{
    public string AccessToken { get; set; } = string.Empty;
}
public class AuthorizeMiddleware : IMiddleware
{
    private readonly string apiKey;
    public AuthorizeMiddleware(IOptions<AuthorizeMiddlewareOptions> options)
    {
        apiKey = options.Value.AccessToken;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad Request");
            return;
        }

        if (authHeaderValue.ToString().Replace("Bearer ", string.Empty) != apiKey)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await next(context);
    }
}
