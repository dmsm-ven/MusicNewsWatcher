
using Microsoft.Extensions.Options;
using MusicNewsWatcher.API.Models;
using PainvenNotificator;
using System.Net;

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
        var path = context.Request.Path.Value ?? "";

        // Пропускаем Swagger и внутренние прокси-эндпоинты без проверки
        if (path.StartsWith("/swagger") || path.StartsWith("/api/internal"))
        {
            await next(context);
            return;
        }

        if (!IPAddress.TryParse(clientIp, out _))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad Request");
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
        {
            _ = notificator.Notify("Попытка доступа к API без API KEY", clientIp);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad Request");
            return;
        }

        if (authHeaderValue.ToString().Replace("Bearer ", string.Empty) != apiKey)
        {
            _ = notificator.Notify("Попытка доступа к API с неверным API KEY", clientIp);

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await next(context);
    }
}
