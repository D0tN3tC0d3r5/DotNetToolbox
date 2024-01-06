namespace ConsoleApplication.Builders;

public class CommandLineApplicationBuilder<TApplication>
    : CommandLineApplicationBuilder<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineInterfaceOptions>
    where TApplication : CommandLineApplication<TApplication> {
    internal CommandLineApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>(string[] args)
    : ApplicationBuilder<TApplication, TBuilder, TOptions>(args)
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new();
