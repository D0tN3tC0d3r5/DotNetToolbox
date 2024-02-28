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
    : ApplicationBase<TApplication, TBuilder>, IRunAsShell
    where TApplication : ShellApplication<TApplication, TBuilder>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder> {
    private readonly bool _allowMultiLine;

    protected ShellApplication(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<ExitCommand>();
        AddCommand<ClearScreenCommand>();
        AddCommand<HelpCommand>();
        _allowMultiLine = Context.TryGetValue("AllowMultiLine", out var isAllowed) && isAllowed is true;
    }

    internal sealed override async Task Run(CancellationToken ct) {
        Environment.Output.WriteLine(FullName);
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

    private Task<Result> ProcessFreeText(string input, CancellationToken ct) {
        if (_allowMultiLine) input += Environment.Input.ReadMultiLine(Enter, Control);
        return ProcessInput(input, ct);
    }

    protected virtual Task<Result> ProcessInput(string input, CancellationToken ct)
        => SuccessTask();

    protected virtual async Task<Result> ExecuteDefault(CancellationToken ct) {
        Environment.Output.Write(GetPrePromptText());
        Environment.Output.WritePrompt();
        var input = Environment.Input.ReadLine() ?? string.Empty;
        var tokens = UserInputParser.Parse(input);
        return StartsWithCommand(tokens.FirstOrDefault())
                         ? await ProcessCommand(tokens, ct).ConfigureAwait(false)
                         : await ProcessFreeText(input!, ct).ConfigureAwait(false);
    }

    private bool StartsWithCommand(string? firstWord)
        => Commands.Any(c => c.Name.Equals(firstWord, StringComparison.OrdinalIgnoreCase)
                          || c.Aliases.Contains(firstWord));
}
