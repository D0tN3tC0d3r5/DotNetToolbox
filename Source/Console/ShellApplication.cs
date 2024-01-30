namespace DotNetToolbox.ConsoleApplication;

public sealed class ShellApplication
    : ShellApplication<ShellApplication> {
    internal ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class ShellApplication<TApplication>(string[] args, string? environment, IServiceProvider serviceProvider)
    : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>(args, environment, serviceProvider)
    where TApplication : ShellApplication<TApplication>;

public abstract class ShellApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {

    protected ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
        AddCommand<ExitCommand>();
        AddCommand<ClearScreenCommand>();
        AddCommand<HelpCommand>();
    }

    internal sealed override async Task Run(CancellationToken ct) {
        Output.WriteLine(FullName);
        var result = await Execute(ct);
        ProcessResult(result);
        if (!result.IsSuccess) {
            ExitWith(DefaultErrorCode);
            return;
        }

        while (IsRunning && !ct.IsCancellationRequested)
            await ProcessCommandLine(ct);
    }

    protected override Task<Result> Execute(CancellationToken ct) => SuccessTask();

    private async Task ProcessCommandLine(CancellationToken ct) {
        Output.Write(Settings.Prompt);
        var userInputText = Input.ReadLine();
        var userInputs = UserInputParser.Parse(userInputText);
        var result = await ProcessUserInput(userInputs, ct);
        ProcessResult(result);
    }
}
