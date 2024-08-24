namespace DotNetToolbox.Http;

public class HttpClientProvider
    : IHttpClientProvider {
    private readonly IHttpClientFactory _clientFactory;

    public HttpClientProvider(string name,
                              IHttpClientFactory clientFactory,
                              IConfiguration configuration,
                              Action<HttpClientOptions>? configure = null,
                              Dictionary<string, object?>? context = null) {
        Name = IsNotNullOrWhiteSpace(name);
        _clientFactory = clientFactory;
        ConfigurationPath = $"{HttpClientOptions.SectionName}:{Name}";
        Options = new();
        EnsureIsConfigured(configuration, configure);
    }

    private void EnsureIsConfigured(IConfiguration configuration,
                                    Action<HttpClientOptions>? configure,
                                    Dictionary<string, object?>? context = null) {
        configuration.GetSection(ConfigurationPath).Bind(Options);
        SetDefaultConfiguration(Options);
        configure?.Invoke(Options);
        var result = Options.Validate(context);
        if (result.HasException) throw result.Exception;
        if (result.HasErrors) throw new ValidationException("Error while creating up the HttpClient provider.", result.Errors);
    }

    public string Name { get; }
    public HttpClientOptions Options { get; }
    protected virtual void SetDefaultConfiguration(HttpClientOptions options) { }

    public string ConfigurationPath { get; }

    public void Authorize(string value, DateTimeOffset? expiresOn = null)
        => Options.Authentication?.Authorize(value, expiresOn);

    public void ExtendAuthorizationUntil(DateTimeOffset expiresOn)
        => Options.Authentication?.ExtendUntil(expiresOn);

    public void RevokeAuthorization()
        => Options.Authentication?.Revoke();

    public HttpClient GetHttpClient() {
        var client = _clientFactory.CreateClient(Name);
        Options.Configure(client);
        return client;
    }
}
