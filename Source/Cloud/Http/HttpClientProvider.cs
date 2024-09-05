namespace DotNetToolbox.Http;

public class HttpClientProvider
    : IHttpClientProvider {
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientProvider(string name,
                              IHttpClientFactory httpClientFactory,
                              IConfiguration configuration,
                              Action<HttpClientOptions>? configure = null,
                              IContext? context = null) {
        Name = IsNotNullOrWhiteSpace(name);
        _httpClientFactory = httpClientFactory;
        ConfigurationPath = $"{HttpClientOptions.SectionName}:{Name}";
        Options = new();
        EnsureIsConfigured(configuration, configure, context);
    }

    private void EnsureIsConfigured(IConfiguration configuration,
                                    Action<HttpClientOptions>? configure,
                                    IContext? context = null) {
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
        var client = _httpClientFactory.CreateClient(Name);
        Options.Configure(client);
        return client;
    }
}
