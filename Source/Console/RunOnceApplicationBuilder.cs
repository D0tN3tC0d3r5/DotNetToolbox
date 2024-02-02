namespace DotNetToolbox.ConsoleApplication;

public class RunOnceApplicationBuilder<TApplication>
    : RunOnceApplicationBuilder<TApplication, RunOnceApplicationBuilder<TApplication>, RunOnceApplicationOptions>
    where TApplication : RunOnceApplication<TApplication> {
    internal RunOnceApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class RunOnceApplicationBuilder<TApplication, TBuilder, TOptions>(string[] args)
    : ApplicationBuilder<TApplication, TBuilder, TOptions>(args)
    where TApplication : RunOnceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : RunOnceApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new();
