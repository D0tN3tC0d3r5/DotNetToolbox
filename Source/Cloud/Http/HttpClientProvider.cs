namespace DotNetToolbox.Http;

public class HttpClientProvider(string provider, IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider<HttpClientOptions>(provider, clientFactory, configuration) {
    public HttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration)
        : this(string.Empty, clientFactory, configuration)
    {
    }

    protected override void SetDefaultConfiguration(HttpClientOptions options) { }
}

public abstract class HttpClientProvider<TOptions>
    : IHttpClientProvider
    where TOptions : HttpClientOptions, new() {
    private readonly string _provider;
    private readonly IHttpClientFactory _clientFactory;
    private readonly TOptions _options;

    protected HttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration)
        : this(string.Empty, clientFactory, configuration) {
    }

    protected HttpClientProvider(string provider, IHttpClientFactory clientFactory, IConfiguration configuration) {
        _provider = provider;
        _clientFactory = clientFactory;
        var providePath = string.IsNullOrWhiteSpace(_provider) ? string.Empty : $":{_provider}";
        ConfigurationPath = $"{HttpClientOptions.SectionName}{providePath}";
        _options = new();
        configuration.GetSection(ConfigurationPath).Bind(_options);
        var optionsValidationContext = new Dictionary<string, object?> { ["Provider"] = _provider };
        _options = IsValid(_options, optionsValidationContext);
        // ReSharper disable once VirtualMemberCallInConstructor - This is intentional.
        SetDefaultConfiguration(_options);
    }

    protected abstract void SetDefaultConfiguration(TOptions options);

    public string ConfigurationPath { get; }

    public void Authorize(string value, DateTimeOffset? expiresOn = null) {
        if (_options.Authentication is null) return;
        _options.Authentication.Authorize(value, expiresOn);
    }

    public void ExtendAuthorizationUntil(DateTimeOffset expiresOn) {
        if (_options.Authentication is null) return;
        _options.Authentication.ExtendUntil(expiresOn);
    }

    public void RevokeAuthorization() {
        if (_options.Authentication is null) return;
        _options.Authentication.Revoke();
    }

    public HttpClient GetHttpClient() {
        var client = _clientFactory.CreateClient(_provider);
        _options.Configure(client);
        return client;
    }
}
