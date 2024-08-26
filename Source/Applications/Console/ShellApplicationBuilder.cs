using IConfigurationBuilder = DotNetToolbox.ConsoleApplication.Application.IConfigurationBuilder;

namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationBuilder<TApplication, TSettings>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : ShellApplicationBuilder<TApplication, ShellApplicationBuilder<TApplication, TSettings>, TSettings>(args, configure)
    where TApplication : ShellApplication<TApplication, TSettings>
    where TSettings : ApplicationSettings, new();

public abstract class ShellApplicationBuilder<TApplication, TBuilder, TSettings>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : ApplicationBuilder<TApplication, TBuilder, TSettings>(args, configure)
    where TApplication : ShellApplication<TApplication, TBuilder, TSettings>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationSettings, new();
