using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API;
using MusicNewsWatcher.API.BackgroundServices;
using MusicNewsWatcher.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

builder.Services.AddMusicNewsWatcherApi(builder.Configuration);
builder.Services.AddTelegramBot(builder.Configuration);

builder.Services.AddHostedService<CrawlerHostedService>();
builder.Services.AddHostedService<TelegramBotHostedService>();

builder.Services.AddScoped<AuthorizeMiddleware>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<AuthorizeMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

