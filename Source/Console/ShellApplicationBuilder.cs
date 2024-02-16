namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationBuilder<TApplication>
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication>>
    where TApplication : ShellApplication<TApplication> {
    internal ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}

public abstract class ShellApplicationBuilder<TApplication, TBuilder>(string[] args)
    : ApplicationBuilder<TApplication, TBuilder>(args)
    where TApplication : ShellApplication<TApplication, TBuilder>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder>;
