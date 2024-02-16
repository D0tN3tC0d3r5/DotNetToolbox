namespace DotNetToolbox.ConsoleApplication;

public class RunOnceApplicationBuilder<TApplication>
    : RunOnceApplicationBuilder<TApplication, RunOnceApplicationBuilder<TApplication>>
    where TApplication : RunOnceApplication<TApplication> {
    internal RunOnceApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class RunOnceApplicationBuilder<TApplication, TBuilder>(string[] args)
    : ApplicationBuilder<TApplication, TBuilder>(args)
    where TApplication : RunOnceApplication<TApplication, TBuilder>
    where TBuilder : RunOnceApplicationBuilder<TApplication, TBuilder>;
