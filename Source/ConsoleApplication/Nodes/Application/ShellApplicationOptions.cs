namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public record ShellApplicationOptions
    : ShellApplicationOptions<ShellApplicationOptions> {
}

public record ShellApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : ShellApplicationOptions<TOptions> {
    public string Prompt { get; init; } = "> ";
}
