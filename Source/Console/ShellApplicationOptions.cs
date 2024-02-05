namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationOptions
    : ShellApplicationOptions<ShellApplicationOptions>;

public abstract class ShellApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {
    public string Prompt { get; set; } = "> ";
}
