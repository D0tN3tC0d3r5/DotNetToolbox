namespace DotNetToolbox.ConsoleApplication;

public sealed class ShellApplication
    : ShellApplication<ShellApplication> {
    internal ShellApplication(string[] args, IServiceProvider services)
        : base(args, services) {
    }
}

public abstract class ShellApplication<TApplication>(string[] args, IServiceProvider services)
    : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>(args, services)
    where TApplication : ShellApplication<TApplication>;

public abstract class ShellApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>, IRunAsShell
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {

    protected ShellApplication(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<ExitCommand>();
        AddCommand<ClearScreenCommand>();
        AddCommand<HelpCommand>();
    }

    internal sealed override async Task Run(CancellationToken ct = default) {
        Environment.Output.WriteLine(FullName);
        var result = await Execute(ct);
        ProcessResult(result);
        if (!result.IsSuccess) {
            ExitWith(DefaultErrorCode);
            return;
        }

        while (IsRunning && !ct.IsCancellationRequested)
            await ProcessCommandLine(ct);
    }

    protected override Task<Result> Execute(CancellationToken ct = default) => SuccessTask();

    private async Task ProcessCommandLine(CancellationToken ct) {
        Environment.Output.Write(Settings.Prompt);
        var userInputText = Environment.Input.ReadLine();
        var userInputs = UserInputParser.Parse(userInputText);
        var result = await ProcessUserInput(userInputs, ct);
        ProcessResult(result);
    }
}
