namespace DotNetToolbox.ConsoleApplication;
public sealed class CommandLineInterfaceApplication
    : CommandLineInterfaceApplication<CommandLineInterfaceApplication> {
    internal CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineInterfaceApplication<TApplication>
    : CommandLineInterfaceApplication<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineInterfaceApplicationOptions>
    where TApplication : CommandLineInterfaceApplication<TApplication> {
    protected CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    protected CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
        AddCommand<HelpOption>();
        AddCommand<VersionOption>();
    }

    protected sealed override async Task<Result> ExecuteAsync(CancellationToken ct) {
        var result = await base.ExecuteAsync(ct);
        if (!InputIsParsed(result)) return result;

        if (Arguments.Length != 0)
            return await ProcessInput(Arguments, ct);

        var helpAction = new HelpOption(this);
        return await helpAction.ExecuteAsync([], ct);
    }
}
