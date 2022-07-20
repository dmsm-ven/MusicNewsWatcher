using MusicNewWatcher.TelegramBot;

public static class Program
{
    static MusicNewsWatcherTelegramBot bot;

    public static async Task Main(string[] args)
    {
        bot = new MusicNewsWatcherTelegramBot(args[0]);

        await bot.StartAsync();

        //await ListenConsoleCommands();
        Console.ReadLine();
    }

    private static async Task ListenConsoleCommands()
    {
        string enteredCommand = string.Empty;
        ShowAvailableCommands();

        do
        {
            enteredCommand = Console.ReadLine() ?? string.Empty;

            if (enteredCommand.StartsWith("send"))
            {
                await SendCommand(enteredCommand);
            }
        } while (enteredCommand != "exit");
    }

    private static async Task SendCommand(string enteredCommand)
    {
        string message = string.Join(" ", enteredCommand.Split(' ').Skip(1));
        if (message.StartsWith("\"") && message.EndsWith("\""))
        {
            message = message.Trim('"');
            await bot.SendTextMessageAsync(message);
        }
    }

    private static void ShowAvailableCommands()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("Available Commands: ");
        Console.WriteLine("1. exit");
        Console.WriteLine("2. send \"message to bot\"");
        Console.ResetColor();
        Console.WriteLine(new String('-', Console.WindowWidth));
    }
}


