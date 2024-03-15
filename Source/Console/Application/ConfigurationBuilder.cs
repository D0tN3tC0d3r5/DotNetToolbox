using MicrosoftConfigurationBuilder = Microsoft.Extensions.Configuration.IConfigurationBuilder;

namespace DotNetToolbox.ConsoleApplication.Application;

public class ConfigurationBuilder<TApplication, TBuilder>(string? environmentName = null)
    : IConfigurationBuilder
    where TApplication : ApplicationBase<TApplication, TBuilder>
    where TBuilder : ApplicationBuilder<TApplication, TBuilder> {
    private readonly string? _environmentName = environmentName ?? string.Empty;

    private bool _addEnvironmentVariables;
    private string? _environmentVariablesPrefix;
    private IDictionary<string, object?> _inMemoryValues = new Dictionary<string, object?>();
    private bool _useAppSettings;
    private IFileProvider? _fileProvider;
    private Type? _userSecretsReference;

    public void AddEnvironmentVariables(string? prefix = null) {
        _addEnvironmentVariables = true;
        _environmentVariablesPrefix = prefix;
    }

    public void AddUserSecrets<TReference>()
        where TReference : class
        => _userSecretsReference = typeof(TReference);

    public void AddAppSettings(IFileProvider? fileProvider = null) {
        _useAppSettings = true;
        _fileProvider = fileProvider;
    }

    public void AddInMemoryValues(IDictionary<string, object?> values)
        => _inMemoryValues = values;

    public IConfigurationRoot Build() {
        var cm = new ConfigurationManager();
        Configure(cm);
        return cm;
    }

    private void Configure(MicrosoftConfigurationBuilder configuration) {
        AddEnvironmentVariables(configuration);
        AddJsonFile(configuration);
        AddUserSecrets(configuration);
        AddInMemoryValues(configuration);
    }

    private void AddUserSecrets(MicrosoftConfigurationBuilder configuration) {
        if (_userSecretsReference is null) return;
        configuration.AddUserSecrets(_userSecretsReference.Assembly, optional: true, reloadOnChange: false);
    }

    private void AddEnvironmentVariables(MicrosoftConfigurationBuilder configuration) {
        if (!_addEnvironmentVariables) return;
        configuration.AddEnvironmentVariables(_environmentVariablesPrefix);
    }

    private void AddInMemoryValues(MicrosoftConfigurationBuilder configuration) {
        if (_inMemoryValues.Count == 0) return;
        configuration.AddJsonStream(new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(_inMemoryValues)));
    }

    private void AddJsonFile(MicrosoftConfigurationBuilder configuration) {
        if (!_useAppSettings) return;
        configuration.AddJsonFile((Action<JsonConfigurationSource>?)ConfigureSource("appsettings.json"));
        if (!string.IsNullOrWhiteSpace(_environmentName)) return;
        configuration.AddJsonFile((Action<JsonConfigurationSource>?)ConfigureSource($"appsettings.{_environmentName}.json", isOptional: true));
    }

    private Action<JsonConfigurationSource> ConfigureSource(string path, bool isOptional = false)
        => s => {
            s.FileProvider = _fileProvider;
            s.Path = path;
            s.Optional = isOptional;
            s.ReloadOnChange = true;
            s.ResolveFileProvider();
        };
}
