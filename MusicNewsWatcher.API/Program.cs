using Microsoft.EntityFrameworkCore;
using MusicNewsWatcher.API;
using MusicNewsWatcher.API.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMusicNewsWatcherApi(builder.Configuration);
builder.Services.AddScoped<AuthorizeMiddleware>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<AuthorizeMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

