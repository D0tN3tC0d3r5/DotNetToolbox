namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public sealed class ShellApplication
    : ShellApplication<ShellApplication> {
    private ShellApplication(string[] args, string? environment, IServiceProvider serviceProvider)
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

    public override async Task<Result> ExecuteAsync(string[] input, CancellationToken ct) {
        await base.ExecuteAsync(input, ct);
        var result = Success();
        while (!ct.IsCancellationRequested && !Terminate) {
            Output.Write(Options.Prompt);
            var userInput = Input.ReadLine() ?? string.Empty;
            var arguments = CommandInputParser.Parse(userInput);
            result = await ProcessInput(arguments, ct);
            if (Terminate) break;
            if (!result.HasException) continue;
            await Output.Error.WriteLineAsync(result.Exception.ToString());
            var exitCode = result.Exception is ConsoleException ce ? ce.ExitCode : 1;
            await ExitAsync(exitCode);
        }

        return result;
    }
}
