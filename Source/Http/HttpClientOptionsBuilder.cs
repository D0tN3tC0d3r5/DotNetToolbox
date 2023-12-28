namespace DotNetToolbox.Http;

public class HttpClientOptionsBuilder(HttpClientOptions? options = null) {

    private readonly HttpClientOptions _options = options ?? new HttpClientOptions();
    //private string? _name;

    public HttpClientOptionsBuilder SetBaseAddress(Uri baseAddress) {
        _options.BaseAddress = baseAddress;
        return this;
    }

    public HttpClientOptionsBuilder SetResponseFormat(string responseFormat) {
        _options.ResponseFormat = IsNotNullOrWhiteSpace(responseFormat);
        return this;
    }

    public HttpClientOptionsBuilder AddCustomHeader(string key, string value) {
        if (_options.CustomHeaders.TryGetValue(key, out var values)) {
            if (values.Contains(value)) return this;
            _options.CustomHeaders[key] = [.. values, value];
            return this;
        }
        _options.CustomHeaders[key] = [value];
        return this;
    }

    public HttpClientOptionsBuilder UseApiKeyAuthentication(Action<ApiKeyAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder UseSimpleTokenAuthentication(Action<StaticTokenAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder UseJsonWebTokenAuthentication(Action<JwtAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder UseOAuth2TokenAuthentication(Action<OAuth2TokenAuthenticationOptions> options, IMsalHttpClientFactory identityClientFactory)
        => SetAuthentication(options, IsNotNull(identityClientFactory));

    internal HttpClientOptions Build()
        => IsValid(_options);

    private HttpClientOptionsBuilder SetAuthentication<T>(Action<T> configAuthentication, IMsalHttpClientFactory? identityClientFactory = null)
        where T : AuthenticationOptions, new() {
        _options.Authentication ??= new T();
        configAuthentication((T)_options.Authentication);
        if (_options.Authentication is OAuth2TokenAuthenticationOptions oAuth2Options)
            oAuth2Options.HttpClientFactory = identityClientFactory;
        return this;
    }
}
