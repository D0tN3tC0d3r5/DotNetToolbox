namespace DotNetToolbox.ConsoleApplication;

public sealed class RunOnceApplication
    : RunOnceApplication<RunOnceApplication> {
    internal RunOnceApplication(string[] args, IServiceProvider services)
        : base(args, services) {
    }
}

public abstract class RunOnceApplication<TApplication>(string[] args, IServiceProvider services)
    : RunOnceApplication<TApplication, RunOnceApplicationBuilder<TApplication>, RunOnceApplicationOptions>(args, services)
    where TApplication : RunOnceApplication<TApplication>;

public abstract class RunOnceApplication<TApplication, TBuilder, TOptions>(string[] args, IServiceProvider services)
    : Application<TApplication, TBuilder, TOptions>(args, services), IRunOnce
    where TApplication : RunOnceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : RunOnceApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {

    internal sealed override async Task Run(CancellationToken ct = default) {
        if (Arguments.Length == 0) {
            await ShowHelp(ct);
            return;
        }

        var result = await Execute(ct);
        ProcessResult(result);
    }

    private Task<Result> ShowHelp(CancellationToken ct = default) {
        var help = new HelpCommand(this);
        return help.Execute(ct);
    }

    protected override Task<Result> Execute(CancellationToken ct = default) => SuccessTask();
}
