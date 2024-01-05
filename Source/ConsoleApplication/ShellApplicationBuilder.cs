namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationBuilder<TApplication>
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>
    where TApplication : ShellApplication<TApplication> {
    internal ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {
    protected ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}
