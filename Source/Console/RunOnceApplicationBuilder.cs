using IConfigurationBuilder = DotNetToolbox.ConsoleApplication.Application.IConfigurationBuilder;

namespace DotNetToolbox.ConsoleApplication;

public class RunOnceApplicationBuilder<TApplication>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : RunOnceApplicationBuilder<TApplication, RunOnceApplicationBuilder<TApplication>>(args, configure)
    where TApplication : RunOnceApplication<TApplication>;

public abstract class RunOnceApplicationBuilder<TApplication, TBuilder>(string[] args, Action<IConfigurationBuilder>? configure = null)
    : ApplicationBuilder<TApplication, TBuilder>(args, configure)
    where TApplication : RunOnceApplication<TApplication, TBuilder>
    where TBuilder : RunOnceApplicationBuilder<TApplication, TBuilder>;
