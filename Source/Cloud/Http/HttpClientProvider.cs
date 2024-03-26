namespace DotNetToolbox.Http;

public class HttpClientProvider(string name, IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider<HttpClientOptions>(name, clientFactory, configuration) {
    public HttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration)
        : this(string.Empty, clientFactory, configuration)
    {
    }

    protected override void SetDefaultConfiguration(HttpClientOptions options) { }
}

public abstract class HttpClientProvider<TOptions>
    : IHttpClientProvider
    where TOptions : HttpClientOptions, new() {
    private readonly IHttpClientFactory _clientFactory;
    private readonly TOptions _options;

    protected HttpClientProvider(string name, IHttpClientFactory clientFactory, IConfiguration configuration) {
        Name = IsNotNullOrWhiteSpace(name);
        _clientFactory = clientFactory;
        ConfigurationPath = $"{HttpClientOptions.SectionName}:{Name}";
        _options = new();
        configuration.GetSection(ConfigurationPath)?.Bind(_options);
        // ReSharper disable once VirtualMemberCallInConstructor - As intended.
        SetDefaultConfiguration(_options);
    }

    public string Name { get; }
    protected abstract void SetDefaultConfiguration(TOptions options);

    public string ConfigurationPath { get; }

    public void Authorize(string value, DateTimeOffset? expiresOn = null)
        => _options.Authentication?.Authorize(value, expiresOn);

    public void ExtendAuthorizationUntil(DateTimeOffset expiresOn)
        => _options.Authentication?.ExtendUntil(expiresOn);

    public void RevokeAuthorization()
        => _options.Authentication?.Revoke();

    public HttpClient GetHttpClient() {
        var client = _clientFactory.CreateClient(Name);
        _options.Configure(client);
        return client;
    }
}
