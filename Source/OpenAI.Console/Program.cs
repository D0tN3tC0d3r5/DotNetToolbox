var builder = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddUserSecrets<Program>(optional: true);
var configuration = builder.Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddSingleton<ILoggerFactory, LoggerFactory>();
services.AddLogging();

CommandRegistry.RegisterCommand(new HelpCommand());
CommandRegistry.RegisterCommand(new ClearScreenCommand());
CommandRegistry.RegisterCommand(new ExitCommand());

Console.WriteLine();
Console.WriteLine("Welcome to OpenAI test console. Type 'exit' to terminate the application.");

while (true) {
    Console.Write("> ");
    var userInput = Console.ReadLine() ?? string.Empty;
    if (string.Equals(userInput, "exit", CurrentCultureIgnoreCase))
        break;

    ParseAndExecute(userInput);
}
