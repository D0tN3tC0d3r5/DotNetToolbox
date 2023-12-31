﻿namespace DotNetToolbox.ConsoleApplication;
public sealed class CommandLineInterfaceApplication
    : CommandLineInterfaceApplication<CommandLineInterfaceApplication> {
    internal CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineInterfaceApplication<TApplication>
    : CommandLineInterfaceApplication<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineInterfaceApplicationOptions>
    where TApplication : CommandLineInterfaceApplication<TApplication> {
    protected CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
    }
}

public abstract class CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    protected CommandLineInterfaceApplication(string[] args, string? environment, IServiceProvider serviceProvider)
        : base(args, environment, serviceProvider) {
        AddAction<HelpAction>();
        AddAction<VersionAction>();
    }

    public sealed override async Task<Result> ExecuteAsync(string[] args, CancellationToken ct) {
        await base.ExecuteAsync(args, ct);
        if (args.Length == 0) {
            var helpAction = Children.OfType<HelpAction>()
                                     .First();
            await helpAction.ExecuteAsync(args, ct);
            return Success();
        }

        var command = Children.OfType<IExecutable>()
                              .FirstOrDefault(c => c.Ids.Contains(args[0], StringComparer.InvariantCultureIgnoreCase));
        if (command is null) return Error($"Command '{args}' not found. For a list of available commands use 'help'.");
        var arguments = args.Length > 1
                            ? args.Skip(1)
                                  .ToArray()
                            : [];
        return await command.ExecuteAsync(arguments, ct);
    }
}
