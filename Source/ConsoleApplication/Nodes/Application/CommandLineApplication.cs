namespace DotNetToolbox.ConsoleApplication.Nodes.Application;
public sealed class CommandLineApplication
    : CommandLineApplication<CommandLineApplication> {
    private CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public class CommandLineApplication<TApplication>
    : CommandLineApplication<TApplication, CommandLineApplicationBuilder<TApplication>, ApplicationOptions>
    where TApplication : CommandLineApplication<TApplication> {
    protected CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    protected CommandLineApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
        AddArgument<HelpAction>();
    }

    public override async Task<Result> ExecuteAsync(string[] input, CancellationToken ct) {
        await base.ExecuteAsync(input, ct);

        if (input.Length == 0) {
            var helpAction = Children.OfType<HelpAction>().First();
            await helpAction.ExecuteAsync(input, ct);
            return Success();
        }

        var command = Children.OfType<IExecutable>().FirstOrDefault(c => c.Ids.Contains(input[0], StringComparer.InvariantCultureIgnoreCase));
        if (command is null) return Error($"Command '{input}' not found. For a list of available commands use 'help'.");
        var arguments = input.Length > 1 ? input.Skip(1).ToArray() : [];
        return await command.ExecuteAsync(arguments, ct);
    }
}
