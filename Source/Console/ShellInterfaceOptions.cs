namespace ConsoleApplication;

public record ShellInterfaceOptions
    : ShellApplicationOptions<ShellInterfaceOptions>;

public abstract record ShellApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : ShellApplicationOptions<TOptions> {
    public string Prompt { get; init; } = "> ";
}
