namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public class ShellApplicationBuilder<TApplication>
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>
    where TApplication : ShellApplication<TApplication> {
    protected ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}

public class ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {
    protected ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}
