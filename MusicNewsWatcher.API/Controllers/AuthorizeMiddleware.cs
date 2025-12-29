
using Microsoft.Extensions.Options;

namespace MusicNewsWatcher.API.Controllers;

public record AuthorizeMiddlewareOptions(string ApiKey);
public class AuthorizeMiddleware : IMiddleware
{
    private readonly string apiKey;
    public AuthorizeMiddleware(IOptions<AuthorizeMiddlewareOptions> options)
    {
        apiKey = options.Value.ApiKey;
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
