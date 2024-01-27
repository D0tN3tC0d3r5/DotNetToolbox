using DotNetToolbox.ConsoleApplication.Application;
using DotNetToolbox.ConsoleApplication.Arguments;

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

    protected sealed override async Task ExecuteInternalAsync(CancellationToken ct) {
        if (Settings.ClearScreenOnStart) Output.ClearScreen();
        Output.WriteLine(FullName);
        if (await HasInvalidArguments(ct)) {
            Exit(DefaultErrorCode);
            return;
        }

        while (IsRunning && !ct.IsCancellationRequested)
            await ProcessCommandLine(ct);
    }

    private async Task ProcessCommandLine(CancellationToken ct) {
        Output.Write(Settings.Prompt);
        var userInputText = Input.ReadLine();
        var userInputs = UserInputParser.Parse(userInputText);
        var result = await ProcessUserInput(userInputs, ct);
        ProcessResult(result);
    }
}
