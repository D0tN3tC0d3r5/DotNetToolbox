namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class ShellApplicationBuilder<TApplication>
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>
    where TApplication : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions> {
    protected ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}

public class ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {
    internal ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}
