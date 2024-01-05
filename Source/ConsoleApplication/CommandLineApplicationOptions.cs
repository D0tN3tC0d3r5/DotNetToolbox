namespace DotNetToolbox.ConsoleApplication;

public record CommandLineApplicationOptions
    : CommandLineApplicationOptions<CommandLineApplicationOptions>;

public abstract record CommandLineApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : CommandLineApplicationOptions<TOptions>;
