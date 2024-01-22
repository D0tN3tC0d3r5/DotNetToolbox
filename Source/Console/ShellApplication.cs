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

    protected sealed override async Task ExecuteInternalAsync(CancellationToken ct) {
        if (Options.ClearScreenOnStart) Output.ClearScreen();
        var result = await ArgumentsReader.Read(Arguments, [.. Children], ct);
        Output.WriteLine(FullName);
        if (!EnsureArgumentsAreValid(result)) {
            Exit(DefaultErrorCode);
            return;
        }

        while (IsRunning && !ct.IsCancellationRequested)
            await ProcessCommandLine(ct);
    }

    private async Task ProcessCommandLine(CancellationToken ct) {
        Output.Write(Options.Prompt);
        var userInputText = Input.ReadLine();
        var userInputs = UserInputParser.Parse(userInputText);
        var result = await ProcessUserInput(userInputs, ct);
        if (Terminate(result)) Exit();
    }
}
