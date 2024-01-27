using DotNetToolbox.ConsoleApplication.Application;

namespace DotNetToolbox.ConsoleApplication;

public class CommandLineInterfaceApplicationOptions
    : CommandLineApplicationOptions<CommandLineInterfaceApplicationOptions>;

public abstract class CommandLineApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : CommandLineApplicationOptions<TOptions>, new();
