using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DotNetToolbox.ConsoleApplication.Application;

public class ApplicationBuilder<TApplication, TBuilder, TSettings>
    : IApplicationBuilder<TApplication, TBuilder, TSettings>
    where TApplication : ApplicationBase<TApplication, TBuilder, TSettings>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TSettings>
    where TSettings : ApplicationSettings, new() {
    private readonly string[] _args;
    private readonly string _environment;

    private Action<ILoggingBuilder> _setLogging = _ => { };
    private IAssemblyDescriptor? _assemblyDescriptor;
    private IDateTimeProvider? _dateTimeProvider;
    private IGuidProvider? _guidProvider;
    private IFileSystemAccessor? _fileSystem;
    private IOutput? _output;
    private IInput? _input;

    public ApplicationBuilder(string[] args, Action<IConfigurationBuilder>? configure = null) {
        _args = args;
        var index = Array.FindIndex(_args, a => a is "--environment" or "-env");
        _environment = (index >= 0 ? _args[index + 1] : null)
                    ?? System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    ?? string.Empty;
        var configurationBuilder = new ConfigurationBuilder<TApplication, TBuilder, TSettings>(_environment);
        configure?.Invoke(configurationBuilder);
        Configuration = configurationBuilder.Build();
    }

    public IServiceCollection Services { get; } = new ServiceCollection();
    public IConfigurationRoot Configuration { get; }

    public void SetAssemblyInformation(IAssemblyDescriptor assemblyDescriptor)
        => _assemblyDescriptor = IsNotNull(assemblyDescriptor);
    public void SetInputHandler(IInput input)
        => _input = IsNotNull(input);
    public void SetOutputHandler(IOutput output)
        => _output = IsNotNull(output);
    public void SetFileSystem(IFileSystemAccessor fileSystem)
        => _fileSystem = IsNotNull(fileSystem);
    public void SetGuidProvider(IGuidProvider guidProvider)
        => _guidProvider = IsNotNull(guidProvider);
    public void SetDateTimeProvider(IDateTimeProvider dateTimeProvider)
        => _dateTimeProvider = IsNotNull(dateTimeProvider);
    public void ConfigureLogging(Action<ILoggingBuilder> configure)
        => _setLogging = IsNotNull(configure);

    public TApplication Build() {
        Services.SetEnvironment(_environment,
                                _assemblyDescriptor,
                                _dateTimeProvider,
                                _guidProvider,
                                _fileSystem,
                                _input,
                                _output);
        Services.TryAddSingleton(Configuration);
        Services.TryAddSingleton<IConfiguration>(Configuration);
        Services.TryAddSingleton<IPromptFactory, PromptFactory>();
        AddLogging(Configuration);

        var serviceProvider = Services.BuildServiceProvider();
        return InstanceFactory.Create<TApplication>(serviceProvider, _args, Services);
    }

    private void AddLogging(IConfiguration configuration)
        => Services.AddLogging(b => {
            b.AddConfiguration(configuration);
            _setLogging(b);
        });
}
