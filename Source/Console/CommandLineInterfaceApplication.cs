namespace DotNetToolbox.ConsoleApplication;

public sealed class CommandLineInterfaceApplication
    : CommandLineInterfaceApplication<CommandLineInterfaceApplication> {
    internal CommandLineInterfaceApplication(string[] args, IServiceProvider services)
        : base(args, services) {
    }
}

public abstract class CommandLineInterfaceApplication<TApplication>
    : CommandLineInterfaceApplication<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineInterfaceApplicationOptions>
    where TApplication : CommandLineInterfaceApplication<TApplication> {
    protected CommandLineInterfaceApplication(string[] args, IServiceProvider services)
        : base(args, services) {
        (this as IHasChildren).AddFlag<VersionFlag>();
    }
}

public abstract class CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>(string[] args, IServiceProvider services)
    : Application<TApplication, TBuilder, TOptions>(args, services)
    where TApplication : CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {

    internal sealed override async Task Run(CancellationToken ct) {
        if (Arguments.Length == 0) {
            await ShowHelp(ct);
            return;
        }

        var result = await Execute(ct);
        ProcessResult(result);
    }

    private Task<Result> ShowHelp(CancellationToken ct) {
        var help = new HelpCommand(this);
        return help.Execute(ct);
    }

    protected override Task<Result> Execute(CancellationToken ct) => SuccessTask();
}

