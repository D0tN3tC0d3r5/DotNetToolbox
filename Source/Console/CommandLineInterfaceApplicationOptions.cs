namespace DotNetToolbox.ConsoleApplication;

public record CommandLineInterfaceApplicationOptions
    : CommandLineApplicationOptions<CommandLineInterfaceApplicationOptions>;

public abstract record CommandLineApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : CommandLineApplicationOptions<TOptions>;
