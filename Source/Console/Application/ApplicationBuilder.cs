namespace DotNetToolbox.ConsoleApplication.Application;

public class ApplicationBuilder<TApplication, TBuilder, TOptions>
    : IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : Application<TApplication, TBuilder, TOptions>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, INamedOptions<TOptions>, new() {
    private readonly string[] _args;
    private string? _environment;
    private bool _addEnvironmentVariables;
    private string? _environmentVariablesPrefix;
    private readonly Dictionary<string, object?> _extraValues = [];
    private bool _useAppSettings;
    private IFileProvider? _fileProvider;
    private Type? _userSecretsReference;
    private Action<ILoggingBuilder> _setLogging = _ => { };
    private Action<TOptions>? _setOptions;
    private IAssemblyDescriptor? _assemblyDescriptor;
    private IDateTimeProvider? _dateTimeProvider;
    private IGuidProvider? _guidProvider;
    private IFileSystem? _fileSystem;
    private IOutput? _output;
    private IInput? _input;

    internal ApplicationBuilder(string[] args) {
        _args = args;
    }

    public ServiceCollection Services { get; } = [];

    public void SetAssemblyInformation(IAssemblyDescriptor assemblyDescriptor) => _assemblyDescriptor = IsNotNull(assemblyDescriptor);
    public void SetInputHandler(IInput input) => _input = IsNotNull(input);
    public void SetOutputHandler(IOutput output) => _output = IsNotNull(output);
    public void SetFileSystem(IFileSystem fileSystem) => _fileSystem = IsNotNull(fileSystem);
    public void SetGuidProvider(IGuidProvider guidProvider) => _guidProvider = IsNotNull(guidProvider);
    public void SetDateTimeProvider(IDateTimeProvider dateTimeProvider) => _dateTimeProvider = IsNotNull(dateTimeProvider);

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

    public TBuilder AddAppSettings(IFileProvider? fileProvider = null) {
        _useAppSettings = true;
        _fileProvider = fileProvider;
        return (TBuilder)this;
    }

    public TBuilder AddValue(string key, object value) {
        _extraValues[key] = value;
        return (TBuilder)this;
    }

    public TBuilder ConfigureOptions(Action<TOptions> configure) {
        _setOptions = IsNotNull(configure);
        return (TBuilder)this;
    }

    public TBuilder ConfigureLogging(Action<ILoggingBuilder> configure) {
        _setLogging = IsNotNull(configure);
        return (TBuilder)this;
    }

    public TApplication Build() {
        var configuration = new ConfigurationManager();
        SetConfiguration(configuration);
        Services.AddEnvironment(_environment,
                                _assemblyDescriptor,
                                _dateTimeProvider,
                                _guidProvider,
                                _fileSystem,
                                _input,
                                _output);
        AddLogging(configuration);

        var serviceProvider = Services.BuildServiceProvider();
        var app = CreateInstance.Of<TApplication>(_args, serviceProvider);
        _setOptions?.Invoke(app.Settings);
        var items = ((IConfigurationManager)configuration).Build().AsEnumerable().ToList();
        foreach (var item in items) app.Context.Add(item.Key, item.Value);
        return app;
    }

    private void SetConfiguration(IConfigurationManager configuration) {
        AddEnvironmentVariables(configuration);
        AddJsonFile(configuration);
        AddUserSecrets(configuration);
        AddConfiguration(configuration);
        AddOptions(configuration);
        Services.AddSingleton<IConfiguration>(configuration);
    }

    private void AddLogging(IConfiguration configuration)
        => Services.AddLogging(b => {
            b.AddConfiguration(configuration);
            _setLogging(b);
        });

    private void AddOptions(IConfiguration configuration) {
        var section = configuration.GetSection(TOptions.SectionName);
        if (!section.Exists()) return;
        Services.Configure<TOptions>(section);
    }

    private void AddUserSecrets(IConfigurationBuilder configuration) {
        if (_userSecretsReference is null) return;
        configuration.AddUserSecrets(_userSecretsReference.Assembly, optional: true, reloadOnChange: false);
    }

    private void AddEnvironmentVariables(IConfigurationBuilder configuration) {
        if (!_addEnvironmentVariables) return;
        configuration.AddEnvironmentVariables(_environmentVariablesPrefix);
    }

    private void AddConfiguration(IConfigurationBuilder configuration)
        => configuration.AddJsonStream(new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(_extraValues)));

    private void AddJsonFile(IConfigurationBuilder configuration) {
        if (!_useAppSettings) return;
        configuration.AddJsonFile(ConfigureSource("appsettings.json"));
        if (!string.IsNullOrWhiteSpace(_environment)) return;
        configuration.AddJsonFile(ConfigureSource($"appsettings.{_environment}.json", isOptional: true));

        return;

        Action<JsonConfigurationSource> ConfigureSource(string path, bool isOptional = false)
            => s => {
                s.FileProvider = _fileProvider;
                s.Path = path;
                s.Optional = isOptional;
                s.ReloadOnChange = true;
                s.ResolveFileProvider();
            };
    }
}
