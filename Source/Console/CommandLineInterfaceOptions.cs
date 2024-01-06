namespace ConsoleApplication;

public record CommandLineInterfaceOptions
    : CommandLineApplicationOptions<CommandLineInterfaceOptions>;

public abstract record CommandLineApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : CommandLineApplicationOptions<TOptions>;
