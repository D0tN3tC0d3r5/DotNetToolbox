namespace DotNetToolbox.ConsoleApplication;

public class CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    : IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : CommandLineApplication<TApplication, TBuilder, TOptions>
    where TBuilder : CommandLineApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, INamedOptions<TOptions>, new() {
    private readonly string[] _arguments;
    private string _environment = string.Empty;
    private bool _addEnvironmentVariables;
    private readonly List<(string Path, bool IsOptional)> _jsonFiles = [];
    private Type? _userSecretsReference;

    internal CommandLineApplicationBuilder(string[] arguments) {
        _arguments = arguments;
    }

    public void SetEnvironment(string environment)
        => _environment = IsNotNullOrWhiteSpace(environment);
    public void AddEnvironmentVariables()
        => _addEnvironmentVariables = true;
    public void AddUserSecrets<TReference>()
        where TReference : class
        => _userSecretsReference = typeof(TReference);
    public void AddJsonFile(string path, bool optional = false)
        => _jsonFiles.Add((path, optional));

    public ServiceCollection Services { get; } = [];

    public TApplication Build() {
        var configuration = new ConfigurationManager();
        configuration.AddCommandLine(_arguments);
        if (_addEnvironmentVariables) configuration.AddEnvironmentVariables();
        foreach ((var path, var isOptional) in _jsonFiles.DistinctBy(i => i.Path))
            configuration.AddJsonFile(path, isOptional);
        if (_userSecretsReference is not null) configuration.AddUserSecrets(_userSecretsReference.Assembly, optional: true, reloadOnChange: false);
        Services.AddSingleton<IConfiguration>(configuration);
        Services.AddOptions<TOptions>()
                .Bind(configuration.GetSection(TOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        var serviceProvider = Services.BuildServiceProvider();
        return Create.Instance<TApplication>(_arguments, _environment, serviceProvider)
            ?? throw new InvalidOperationException("Failed to create application instance.");
    }
}

public class CommandLineApplicationBuilder
    : CommandLineApplicationBuilder<CommandLineApplication, CommandLineApplicationBuilder, ApplicationOptions>  {
    internal CommandLineApplicationBuilder(string[] arguments)
        : base(arguments) {
    }
}
