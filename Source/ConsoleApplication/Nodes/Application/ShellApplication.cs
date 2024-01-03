namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class ShellApplication<TApplication>
    : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>
    where TApplication : ShellApplication<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions> {
    protected ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class ShellApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new() {

    internal ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }

    public sealed override async Task<int> RunAsync(CancellationToken ct = default) {
        var taskRun = CancellationTokenSource.CreateLinkedTokenSource(ct);
        await base.RunAsync(taskRun.Token);
        while (!taskRun.IsCancellationRequested) {
            Output.Write(Options.Prompt);
            var userInput = Input.ReadLine() ?? string.Empty;
            var result = await ProcessInput(userInput, taskRun.Token);
            if (Terminate) break;
            if (!result.HasException) continue;
            await Output.Error.WriteLineAsync(result.Exception.ToString());
            var exitCode = result.Exception is ConsoleException ce ? ce.ExitCode : 1;
            await ExitAsync(exitCode);
        }

        return ExitCode;
    }

    public virtual async Task<Result> ProcessInput(string input, CancellationToken ct) {
        var command = Children.OfType<ICommand>().FirstOrDefault(c => c.Ids.Contains(input, StringComparer.InvariantCultureIgnoreCase));
        if (command is null) return Result.Error($"Command '{input}' not found. For a list of available commands use 'help'.");
        return await command.ExecuteAsync(Arguments, ct);
    }
}

public class ShellApplication
    : ShellApplication<ShellApplication> {
    internal ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {

        AddCommand<ExitCommand>();
        AddCommand<ClearScreenCommand>();
    }

    public override Task<Result> ProcessInput(string input, CancellationToken ct) => base.ProcessInput(input, ct);
}
