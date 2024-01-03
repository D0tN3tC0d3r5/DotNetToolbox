namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public class ApplicationBuilder<TApplication, TBuilder, TOptions>
    : IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : Application<TApplication, TBuilder, TOptions>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    private readonly string[] _args;
    private string? _environment;
    private bool _addEnvironmentVariables;
    private string? _environmentVariablesPrefix;
    private bool _useAppSettings;
    private Type? _userSecretsReference;
    private Action<ILoggingBuilder> _setLogging = _ => { };
    private string _sectionName = "Application";

    internal ApplicationBuilder(string[] args) {
        _args = args;
    }

    public ServiceCollection Services { get; } = [];

    public TBuilder SetConfigurationSectionName(string sectionName) {
        _sectionName = IsNotNullOrWhiteSpace(sectionName);
        return (TBuilder)this;
    }

    public TBuilder SetEnvironment(string environment) {
        _environment = IsNotNullOrWhiteSpace(environment);
        return (TBuilder)this;
    }

    public TBuilder AddEnvironmentVariables(string? prefix) {
        _addEnvironmentVariables = true;
        _environmentVariablesPrefix = prefix;
        return (TBuilder)this;
    }

    public TBuilder AddUserSecrets<TReference>()
        where TReference : class {
        _userSecretsReference = typeof(TReference);
        return (TBuilder)this;
    }

    public TBuilder AddSettings() {
        _useAppSettings = true;
        return (TBuilder)this;
    }

    public TBuilder SetLogging(Action<ILoggingBuilder>? configure = null) {
        _setLogging = configure ?? _setLogging;
        return (TBuilder)this;
    }

    public TApplication Build() {
        var configuration = new ConfigurationManager();
        configuration.AddCommandLine(_args);
        if (_addEnvironmentVariables)
            configuration.AddEnvironmentVariables(_environmentVariablesPrefix);

        if (_useAppSettings) {
            configuration.AddJsonFile("appsettings.json", optional: false);
            if (string.IsNullOrWhiteSpace(_environment))
                configuration.AddJsonFile($"appsettings.{_environment}.json", optional: true);
        }

        if (_userSecretsReference is not null)
            configuration.AddUserSecrets(_userSecretsReference.Assembly, optional: true, reloadOnChange: false);

        Services.AddSystemUtilities();
        Services.AddSingleton<IConfiguration>(configuration);
        Services.AddSingleton<ILoggerFactory, LoggerFactory>();

        Services.AddLogging(_setLogging);

        var section = configuration.GetSection(_sectionName);
        if (section.Exists()) {
            Services.AddOptions<TOptions>()
                    .Bind(section)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
        }

        var serviceProvider = Services.BuildServiceProvider();
        return CreateInstance.Of<TApplication>(_args, _environment, serviceProvider)
            ?? throw new InvalidOperationException("Failed to create application instance.");
    }
}
