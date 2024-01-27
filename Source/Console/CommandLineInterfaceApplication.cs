namespace DotNetToolbox.ConsoleApplication;
public sealed class CommandLineInterfaceApplication
    : CommandLineInterfaceApplication<CommandLineInterfaceApplication> {
    internal CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineInterfaceApplication<TApplication>(string[] args, string? environment, IServiceProvider serviceProvider)
    : CommandLineInterfaceApplication<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineInterfaceApplicationOptions>(args, environment, serviceProvider)
    where TApplication : CommandLineInterfaceApplication<TApplication>;

public abstract class CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>(string[] args, string? environment, IServiceProvider serviceProvider)
    : Application<TApplication, TBuilder, TOptions>(args, environment, serviceProvider)
    where TApplication : CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    protected override async Task ExecuteInternalAsync(CancellationToken ct) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        if (Arguments.Length == 0) {
            var helpAction = new HelpOption(this);
            await helpAction.ExecuteAsync([], ct);
        }

        if (await HasInvalidArguments(ct)) {
            Exit(DefaultErrorCode);
            return;
        }
        await ExecuteAsync(ct);
        Exit();
    }

    protected virtual Task ExecuteAsync(CancellationToken ct) => Task.CompletedTask;
}

