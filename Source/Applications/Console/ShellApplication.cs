namespace DotNetToolbox.ConsoleApplication;

public sealed class ShellApplication
    : ShellApplication<ShellApplication> {
    internal ShellApplication(string[] args, IServiceProvider services)
        : base(args, services) {
    }
}

public abstract class ShellApplication<TApplication>(string[] args, IServiceProvider services)
    : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>>(args, services)
    where TApplication : ShellApplication<TApplication>;

public abstract class ShellApplication<TApplication, TBuilder>
    : ApplicationBase<TApplication, TBuilder>,
      IRunAsShell
    where TApplication : ShellApplication<TApplication, TBuilder>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder> {

    protected ShellApplication(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<ExitCommand>();
        AddCommand<ClearScreenCommand>();
        AddCommand<HelpCommand>();
    }
    protected bool AllowMultiLine { get; set; }

    internal sealed override async Task Run(CancellationToken ct = default) {
        ConsoleOutput.WriteLine(FullName);
        var result = await OnStart(ct).ConfigureAwait(false);
        ProcessResult(result);
        if (!result.IsSuccess) {
            Exit();
            return;
        }

        while (IsRunning && !ct.IsCancellationRequested)
            await ExecuteDefault(ct).ConfigureAwait(false);
    }

    protected virtual Task<Result> OnStart(CancellationToken ct = default) => SuccessTask();

    protected virtual string GetPrePromptText() => string.Empty;

    private async Task<Result> ProcessInput(string input, CancellationToken ct) {
        var lines = input.Split(ConsoleOutput.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (AllowMultiLine && lines.Length > 1)
            return await ProcessFreeText(lines, ct).ConfigureAwait(false);
        var tokens = UserInputParser.Parse(input);
        return StartsWithCommand(tokens.FirstOrDefault())
                   ? await ProcessCommand(tokens, ct).ConfigureAwait(false)
                   : await ProcessFreeText(lines, ct).ConfigureAwait(false);
    }

    protected virtual Task<Result> ProcessFreeText(string[] lines, CancellationToken ct = default)
        => SuccessTask();

    protected virtual Task<Result> ExecuteDefault(CancellationToken ct = default) {
        ConsoleOutput.Write(GetPrePromptText());
        ConsoleOutput.WritePrompt();
        var input = AllowMultiLine
                        ? ConsoleInput.ReadMultiLine(Enter, Control)
                        : ConsoleInput.ReadLine() ?? string.Empty;
        return ProcessInput(input, ct);
    }

    private bool StartsWithCommand(string? firstWord)
        => Commands.Any(c => c.Name.Equals(firstWord, StringComparison.OrdinalIgnoreCase)
                          || c.Aliases.Contains(firstWord));
}
