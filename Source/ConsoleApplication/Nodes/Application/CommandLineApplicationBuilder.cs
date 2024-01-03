namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public class CommandLineApplicationBuilder<TApplication>
    : CommandLineApplicationBuilder<TApplication, CommandLineApplicationBuilder<TApplication>, ApplicationOptions>
    where TApplication : CommandLineApplication<TApplication, CommandLineApplicationBuilder<TApplication>, ApplicationOptions> {
    internal CommandLineApplicationBuilder(string[] args)
        : base(args) {
    }
}

public class CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    internal CommandLineApplicationBuilder(string[] args)
        : base(args) {
    }
}
