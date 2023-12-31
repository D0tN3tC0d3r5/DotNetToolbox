﻿namespace DotNetToolbox.ConsoleApplication;

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

    public sealed override async Task<Result> ExecuteAsync(string[] args, CancellationToken ct) {
        var result = await base.ExecuteAsync(args, ct);
        if (result.HasException) {
            var exitCode = result.Exception is ConsoleException ce ? ce.ExitCode : 1;
            Exit(exitCode);
            return result;
        }

        if (result.HasErrors) {
            foreach (var error in result.Errors)
                Output.WriteLine($"Error: {error}");
            Exit(1);
            return result;
        }

        while (IsRunning && !ct.IsCancellationRequested) {
            Output.Write(Options.Prompt);
            var userInputText = Input.ReadLine() ?? string.Empty;
            var userInputs = CommandInputParser.Parse(userInputText);
            result = await ProcessUserInput(userInputs, ct);
            if (result.HasException) {
                var exitCode = result.Exception is ConsoleException ce ? ce.ExitCode : 1;
                Exit(exitCode);
                return result;
            }

            if (!result.HasErrors) continue;
            foreach (var error in result.Errors)
                Output.WriteLine($"Error: {error}");
        }

        return result;
    }

    private async Task<Result> ProcessUserInput(string[] input, CancellationToken ct) {
        if (input.Length == 0) return Success();
        var command = Children.OfType<IExecutable>().FirstOrDefault(c => c.Ids.Contains(input.First(), StringComparer.InvariantCultureIgnoreCase));
        if (command is null) return Error($"Command '{input}' not found. For a list of available commands use 'help'.");
        var arguments = input.Length > 1 ? input.Skip(1).ToArray() : [];
        return await command.ExecuteAsync(arguments, ct);
    }
}
