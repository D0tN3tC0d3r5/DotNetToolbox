namespace DotNetToolbox.ConsoleApplication;

public abstract class ShellApplication<TApplication, TBuilder, TOptions>
    : Application<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, INamedOptions<TOptions>, new() {
    internal ShellApplication(string[] args, string environment, IServiceProvider serviceProvider) : base(args, environment, serviceProvider) {
    }

    public sealed override async Task<int> RunAsync() {
        var taskRun = new CancellationTokenSource();
        while (!taskRun.IsCancellationRequested) {
            Console.Write("> ");
            var userInput = Console.ReadLine() ?? string.Empty;
            if (string.Equals(userInput, "exit", CurrentCultureIgnoreCase))
                break;

            var result = await ProcessInput(userInput);
            if (!result.HasException) continue;
            Console.WriteLine(result.Exception.ToString());
            return 1;
        }

        return 0;
    }

    public abstract Task<Result> ProcessInput(string input);
}

public abstract class ShellApplication
    : ShellApplication<ShellApplication, ShellApplicationBuilder, ApplicationOptions> {
    internal ShellApplication(string[] args, string environment, IServiceProvider serviceProvider) : base(args, environment, serviceProvider) {
    }
}
