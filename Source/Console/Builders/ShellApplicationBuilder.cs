namespace DotNetToolbox.ConsoleApplication.Builders;

public class ShellApplicationBuilder<TApplication>(string[] args)
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication>, ShellApplicationOptions>(args)
    where TApplication : ShellApplication<TApplication>;

public abstract class ShellApplicationBuilder<TApplication, TBuilder, TOptions>(string[] args)
    : ApplicationBuilder<TApplication, TBuilder, TOptions>(args)
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ShellApplicationOptions<TOptions>, new();
