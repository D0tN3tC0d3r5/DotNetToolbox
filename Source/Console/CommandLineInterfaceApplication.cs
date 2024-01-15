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

    protected override async Task ExecuteInternalAsync(CancellationToken ct) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        if (Arguments.Length == 0) {
            var helpAction = new HelpOption(this);
            await helpAction.ExecuteAsync([], ct);
        }

        var result = await ArgumentsReader.Read(Arguments, [.. Children], ct);
        EnsureArgumentsAreValid(result);
        await ExecuteAsync(ct);
    }

    protected virtual Task ExecuteAsync(CancellationToken ct) => Task.CompletedTask;
}
