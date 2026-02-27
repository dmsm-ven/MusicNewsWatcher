
using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.Models;
using PainvenNotificator;

namespace MusicNewsWatcher.API.Controllers;

public class AuthorizeMiddleware : IMiddleware
{
    private readonly string apiKey;
    private readonly IApiEventNotificator notificator;

    public AuthorizeMiddleware(IOptions<AuthorizeMiddlewareOptions> options, IApiEventNotificator notificator)
    {
        apiKey = options.Value.AccessToken;
        this.notificator = notificator;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string clientIp = context.Request.Headers.ContainsKey("X-Real-IP") ? context.Request.Headers["X-Real-IP"].ToString() : "";

        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
        {
            _ = notificator.Notify($"Unauthorized access attempt from IP: {clientIp}");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad Request");
            return;
        }

        if (authHeaderValue.ToString().Replace("Bearer ", string.Empty) != apiKey)
        {
            _ = notificator.Notify($"Invalid access token auth attempt from IP: {clientIp}");

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await next(context);
    }
}
