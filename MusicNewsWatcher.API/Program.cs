using MusicNewsWatcher.API;
using MusicNewsWatcher.API.BackgroundServices;
using MusicNewsWatcher.API.Controllers;
using MusicNewsWatcher.API.Models;
using PainvenNotificator;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();



builder.Services.Configure<CrawlerConfiguration>(builder.Configuration.GetSection(nameof(CrawlerConfiguration)));

builder.Services.AddMusicNewsWatcherApi(builder.Configuration);
builder.Services.AddTelegramBot(builder.Configuration);
builder.Services.AddTelegramNotificationsWithGeodata(builder.Configuration, options =>
{
    options.AppPrefix = "MusicNewsWatcher";
});

builder.Services.AddHostedService<CrawlerHostedService>();
builder.Services.AddHostedService<TelegramBotHostedService>();

builder.Services.AddScoped<AuthorizeMiddleware>();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Response;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
        p => p.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});



var app = builder.Build();

app.UseHttpsRedirection();

// Enable CORS early so browser preflight requests are handled before other middleware
app.UseCors("cors");

app.UseMiddleware<AuthorizeMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    _ = Task.Run(async () =>
    {
        var notificator = app.Services.GetRequiredService<IApiEventNotificator>();
        await notificator.Notify($"API запущен");
    });
});

app.UseCors("react");
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

