using MusicNewsWatcher.TelegramBot;

public static class Program
{
    static MusicNewsWatcherTelegramBot bot;

    public static async Task Main(string[] args)
    {
        bot = new MusicNewsWatcherTelegramBot(args[0], long.Parse(args[1]));

        bot.Start();

        await ListenConsoleCommands();
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
            else if(enteredCommand == "exit")
            {
                break;
            }
            
        } while (true);
    }

    private static async Task SendCommand(string enteredCommand)
    {
        string message = enteredCommand.Substring(enteredCommand.IndexOf(" ") + 1);
        if (message.StartsWith("\"") && message.EndsWith("\""))
        {
            message = message.Trim('"');
            try
            {
                await bot.SendAsBot(message);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex.Message);
            }
        }
    }

    private static void WriteErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    private static void ShowAvailableCommands()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("Available Commands: ");
        Console.WriteLine("1. exit");
        Console.WriteLine("2. send \"message from bot\"");
        Console.ResetColor();
        Console.WriteLine(new String('-', Console.WindowWidth));
    }
}


