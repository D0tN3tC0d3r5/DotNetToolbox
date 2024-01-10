namespace DotNetToolbox.ConsoleApplication.Builders;

public class CommandLineApplicationBuilder<TApplication>
    : CommandLineApplicationBuilder<TApplication, CommandLineApplicationBuilder<TApplication>, CommandLineInterfaceApplicationOptions>
    where TApplication : CommandLineInterfaceApplication<TApplication> {
    internal CommandLineApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>(string[] args)
    : ApplicationBuilder<TApplication, TBuilder, TOptions>(args)
    where TApplication : CommandLineInterfaceApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new();
