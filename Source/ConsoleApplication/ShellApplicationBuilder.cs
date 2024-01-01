namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, INamedOptions<TOptions>, new() {
    internal ShellApplicationBuilder(string[] arguments)
        : base(arguments) {
    }
}

public class ShellApplicationBuilder
    : ShellApplicationBuilder<ShellApplication, ShellApplicationBuilder, ApplicationOptions> {
    internal ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}
