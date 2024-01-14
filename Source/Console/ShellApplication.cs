namespace DotNetToolbox.ConsoleApplication;

public sealed class ShellApplication
    : ShellApplication<ShellApplication> {
    internal ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class ShellApplication<TApplication>
    : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>
    where TApplication : ShellApplication<TApplication> {
    protected ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

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

    protected sealed override async Task<Result> ExecuteAsync(CancellationToken ct) {
        var result = await base.ExecuteAsync(ct);
        if (!InputIsParsed(result)) return result;

        while (IsRunning && !ct.IsCancellationRequested) {
            Output.Write(Options.Prompt);
            var userInputText = Input.ReadLine() ?? string.Empty;
            var userInputs = UserInputParser.Parse(userInputText);
            result = await ProcessInput(userInputs, ct);
            if (!CanContinue(result)) return result;
        }

        return result;
    }
}
