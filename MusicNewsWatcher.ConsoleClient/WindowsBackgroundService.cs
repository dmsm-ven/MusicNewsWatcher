using Microsoft.Extensions.Hosting;
using MusicNewsWatcher.Core;

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly MusicUpdateManager updateManager;

    public WindowsBackgroundService(MusicUpdateManager updateManager)
    {
        this.updateManager = updateManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await updateManager.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Environment.Exit(1);
        }
    }
}

