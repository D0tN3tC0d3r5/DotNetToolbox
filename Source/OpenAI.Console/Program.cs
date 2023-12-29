using DotNetToolbox.OpenAI.Commands;

var builder = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddUserSecrets<Program>(optional: true);
var configuration = builder.Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddSingleton<ILoggerFactory, LoggerFactory>();
services.AddLogging();

using var main = new MainCommand();
await main.ExecuteAsync(args);
