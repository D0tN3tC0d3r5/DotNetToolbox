namespace DotNetToolbox.Http;

internal class HttpClientOptionsBuilder : IHttpClientOptionsBuilder {

    private readonly IMsalHttpClientFactory _identityClientFactory;
    private readonly HttpClientOptions _options;
    private readonly HttpClientConfiguration _configuration;

    internal HttpClientOptionsBuilder(string? name, HttpClientConfiguration configuration, IMsalHttpClientFactory identityClientFactory) {
        _configuration = IsNotNull(configuration);
        _identityClientFactory = IsNotNull(identityClientFactory);
        _options = _configuration.ResolveOptionsFor(name);
    }

    public IHttpClientOptionsBuilder SetBaseAddress(string baseAddress) {
        _options.BaseAddress = baseAddress;
        return this;
    }

    public IHttpClientOptionsBuilder SetResponseFormat(string responseFormat) {
        _options.ResponseFormat = responseFormat;
        return this;
    }

    public IHttpClientOptionsBuilder AddCustomHeader(string key, string value) {
        if (_options.CustomHeaders.TryGetValue(key, out var values)) {
            if (values.Contains(value)) return this;
            _options.CustomHeaders[key] = values.Append(value).ToArray();
            return this;
        }
        _options.CustomHeaders[key] = new[] { value, };
        return this;
    }

    public IHttpClientOptionsBuilder UseApiKeyAuthentication(Action<ApiKeyAuthenticationOptions> options)
        => SetAuthentication(options);

    public IHttpClientOptionsBuilder UseSimpleTokenAuthentication(Action<StaticTokenAuthenticationOptions> options)
        => SetAuthentication(options);

    public IHttpClientOptionsBuilder UseJsonWebTokenAuthentication(Action<JwtAuthenticationOptions> options)
        => SetAuthentication(options);

    public IHttpClientOptionsBuilder UseOAuth2TokenAuthentication(Action<OAuth2TokenAuthenticationOptions> options)
        => SetAuthentication(options);

    internal HttpClientOptions Build() {
        _options.Validate().EnsureIsValid();
        return _options;
    }

    private IHttpClientOptionsBuilder SetAuthentication<T>(Action<T> configAuthentication)
        where T : AuthenticationOptions, new() {
        _options.Authentication = _configuration.Authentication ?? new T();
        configAuthentication((T)_options.Authentication);
        if (_options.Authentication is OAuth2TokenAuthenticationOptions oAuth2Options)
            oAuth2Options.HttpClientFactory = _identityClientFactory;
        return this;
    }
}