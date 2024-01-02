namespace DotNetToolbox.ConsoleApplication;

public class CommandLineApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    internal CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }

    public override Task<int> RunAsync() => throw new NotImplementedException();
}

public class CommandLineApplication
    : CommandLineApplication<CommandLineApplication, CommandLineApplicationBuilder, ApplicationOptions> {
    internal CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider)
    {
    }
}
