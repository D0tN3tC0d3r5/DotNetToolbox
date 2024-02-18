namespace DotNetToolbox.ConsoleApplication;

public sealed class RunOnceApplication
    : RunOnceApplication<RunOnceApplication> {
    internal RunOnceApplication(string[] args, IServiceProvider services)
        : base(args, services) {
    }
}

public abstract class RunOnceApplication<TApplication>(string[] args, IServiceProvider services)
    : RunOnceApplication<TApplication, RunOnceApplicationBuilder<TApplication>>(args, services)
    where TApplication : RunOnceApplication<TApplication>;

public abstract class RunOnceApplication<TApplication, TBuilder>(string[] args, IServiceProvider services) : ApplicationBase<TApplication, TBuilder>(args, services), IRunOnce
    where TApplication : RunOnceApplication<TApplication, TBuilder>
    where TBuilder : RunOnceApplicationBuilder<TApplication, TBuilder> {
    internal sealed override async Task Run(CancellationToken ct) {
        if (Arguments.Length == 0) {
            await ShowHelp(ct);
            return;
        }

        var result = await ExecuteDefault(ct);
        ProcessResult(result);
    }

    protected virtual Task<Result> ExecuteDefault(CancellationToken ct) => SuccessTask();

    private Task<Result> ShowHelp(CancellationToken ct) {
        var help = new HelpCommand(this);
        return help.Execute(ct);
    }
}
