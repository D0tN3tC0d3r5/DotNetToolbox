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
        _allowMultiLine = Context.TryGetValue("AllowMultiLine", out var isAllowed) && isAllowed == "true";
    }

    internal sealed override async Task Run(CancellationToken ct) {
        var result = await OnStart(ct);
        ProcessResult(result);
        if (!result.IsSuccess) {
            ExitWith(DefaultErrorCode);
            return;
        }

        while (IsRunning && !ct.IsCancellationRequested)
            await ProcessInput(ct);
    }

    protected virtual Task<Result> OnStart(CancellationToken ct) {
        Environment.Output.WriteLine(FullName);
        return SuccessTask();
    }

    private async Task ProcessInput(CancellationToken ct) {
        Environment.Output.WritePrompt();
        var input = Environment.Input.ReadLine();
        var tokens = UserInputParser.Parse(input);
        var result = StartsWithCommand(tokens.FirstOrDefault())
                     ? await ProcessCommand(tokens, ct)
                     : await ProcessFreeText(input, ct);

        ProcessResult(result);
    }

    private Task<Result> ProcessFreeText(string input, CancellationToken ct) {
        if (_allowMultiLine) input += Environment.Input.ReadMultiLine(Enter, Control);
        return ExecuteDefault(input, ct);
    }

    protected virtual Task<Result> ExecuteDefault(string input, CancellationToken ct) => SuccessTask();

    private bool StartsWithCommand(string? firstWord)
        => Commands.Any(c => c.Name.Equals(firstWord, StringComparison.OrdinalIgnoreCase)
                          || c.Aliases.Contains(firstWord));
}
