using IConfigurationBuilder = DotNetToolbox.ConsoleApplication.Application.IConfigurationBuilder;

namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationBuilder<TApplication>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication>>(args, configure)
    where TApplication : ShellApplication<TApplication>;

public abstract class ShellApplicationBuilder<TApplication, TBuilder>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : ApplicationBuilder<TApplication, TBuilder>(args, configure)
    where TApplication : ShellApplication<TApplication, TBuilder>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder>;
