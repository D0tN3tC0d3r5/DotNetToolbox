namespace DotNetToolbox.Http;

public abstract class HttpClientProvider
    : IHttpClientProvider {
    private readonly IHttpClientFactory _clientFactory;

    protected HttpClientProvider(string name, IHttpClientFactory clientFactory, IConfiguration configuration) {
        Name = IsNotNullOrWhiteSpace(name);
        _clientFactory = clientFactory;
        ConfigurationPath = $"{HttpClientOptions.SectionName}:{Name}";
        Options = new();
        configuration.GetSection(ConfigurationPath)?.Bind(Options);
        // ReSharper disable once VirtualMemberCallInConstructor - As intended.
        SetDefaultConfiguration(Options);
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
