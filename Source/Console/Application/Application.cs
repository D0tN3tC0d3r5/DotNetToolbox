﻿namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class Application<TApplication, TBuilder, TSettings>
    : ApplicationBase, IApplication<TApplication, TBuilder, TSettings>
    where TApplication : Application<TApplication, TBuilder, TSettings>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationOptions<TSettings>
  , IHasDefault<TSettings>
  , new() {
    private int _exitCode = DefaultExitCode;

    protected Application(string[] args, IServiceProvider services)
        : base(args, services) {
        var settings = Services.GetService<IOptions<TSettings>>();
        Settings = settings?.Value ?? new TSettings();
    }

    public TSettings Settings { get; }

    protected virtual ValueTask Dispose()
        => ValueTask.CompletedTask;

    public static TApplication Create(Action<TBuilder>? configureBuilder = null)
        => Create([], configureBuilder);

    public static TApplication Create(string[] args, Action<TBuilder>? configure = null) {
        var builder = CreateInstance.Of<TBuilder>((object)args);
        configure?.Invoke(builder);
        return builder.Build();
    }

    public override string ToString()
        => $"{GetType().Name}: {Name} v{Version} => {Description}";

    public sealed override void ExitWith(int exitCode = 0) {
        _exitCode = exitCode;
        IsRunning = false;
    }

    public int Run() => RunAsync().GetAwaiter().GetResult();

    public async Task<int> RunAsync() {
        try {
            IsRunning = true;
            var taskRun = new CancellationTokenSource();
            if (Settings.ClearScreenOnStart) Environment.Output.ClearScreen();
            if (await TryParseArguments(taskRun.Token)) return DefaultErrorCode;
            if (!IsRunning) return _exitCode;
            await Run(taskRun.Token);
            return _exitCode;
        }
        catch (ConsoleException ex) {
            Environment.Output.WriteLine(FormatException(ex));
            return ex.ExitCode;
        }
        catch (Exception ex) {
            Environment.Output.WriteLine(FormatException(ex));
            return DefaultErrorCode;
        }
    }

    protected void ProcessResult(Result result) {
        if (result.HasException) throw result.Exception!;
        if (!result.HasErrors) return;
        Environment.Output.WriteLine(FormatValidationErrors(result.Errors));
    }

    protected async Task<bool> TryParseArguments(CancellationToken ct) {
        var result = await ArgumentsParser.Parse(this, Arguments, ct);
        ProcessResult(result);
        return result.HasErrors;
    }

    protected async Task<Result> ProcessUserInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = FindCommand((this as IHasChildren).Commands, input[0]);
        if (command is null) return Invalid($"Command '{input[0]}' not found. For a list of available commands use 'help'.");
        var arguments = input.Skip(1).ToArray();
        return await command.Set(arguments, ct);
    }

    private static ICommand? FindCommand(IEnumerable<ICommand> commands, string token)
        => token.StartsWith('"') || token.StartsWith('-')
               ? null
               : commands.FirstOrDefault(c => c.Name.Equals(token, StringComparison.CurrentCultureIgnoreCase)
                                           || c.Aliases.Contains(token));

    public async ValueTask DisposeAsync() {
        await Dispose();
        GC.SuppressFinalize(this);
    }
}
