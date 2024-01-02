namespace DotNetToolbox.ConsoleApplication;

public class CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new () {
    internal CommandLineApplicationBuilder(string[] arguments)
        : base(arguments) {
    }
}

public class CommandLineApplicationBuilder
    : CommandLineApplicationBuilder<CommandLineApplication, CommandLineApplicationBuilder, ApplicationOptions>  {
    internal CommandLineApplicationBuilder(string[] arguments)
        : base(arguments) {
    }
}
