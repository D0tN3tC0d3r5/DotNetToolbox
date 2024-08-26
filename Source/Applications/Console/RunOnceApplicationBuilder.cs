using IConfigurationBuilder = DotNetToolbox.ConsoleApplication.Application.IConfigurationBuilder;

namespace DotNetToolbox.ConsoleApplication;

public class RunOnceApplicationBuilder<TApplication, TSettings>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : RunOnceApplicationBuilder<TApplication, RunOnceApplicationBuilder<TApplication, TSettings>, TSettings>(args, configure)
    where TApplication : RunOnceApplication<TApplication, TSettings>
    where TSettings : ApplicationSettings, new();

public abstract class RunOnceApplicationBuilder<TApplication, TBuilder, TSettings>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : ApplicationBuilder<TApplication, TBuilder, TSettings>(args, configure)
    where TApplication : RunOnceApplication<TApplication, TBuilder, TSettings>
    where TBuilder : RunOnceApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationSettings, new();
