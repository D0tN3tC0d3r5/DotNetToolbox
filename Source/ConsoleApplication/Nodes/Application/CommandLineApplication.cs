namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class CommandLineApplication<TApplication>
    : CommandLineApplication<TApplication, CommandLineApplicationBuilder<TApplication>, ApplicationOptions>
    where TApplication : CommandLineApplication<TApplication, CommandLineApplicationBuilder<TApplication>, ApplicationOptions> {
    protected CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    internal CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public class CommandLineApplication
    : CommandLineApplication<CommandLineApplication> {
    internal CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }

    public override Task<int> RunAsync(CancellationToken ct = default) => Task.FromResult(0);
}
