namespace DotNetToolbox.ConsoleApplication;

public record ShellApplicationOptions
    : ShellApplicationOptions<ShellApplicationOptions>;

public abstract record ShellApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : ShellApplicationOptions<TOptions> {
    public string Prompt { get; init; } = "> ";
}
