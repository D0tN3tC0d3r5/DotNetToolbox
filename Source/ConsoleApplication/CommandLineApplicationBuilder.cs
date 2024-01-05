namespace DotNetToolbox.ConsoleApplication;

public class CommandLineApplicationBuilder<TApplication>
    : CommandLineApplicationBuilder<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineApplicationOptions>
    where TApplication : CommandLineApplication<TApplication> {
    internal CommandLineApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    protected CommandLineApplicationBuilder(string[] args)
        : base(args) {
    }
}
