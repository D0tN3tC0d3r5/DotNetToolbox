namespace DotNetToolbox.Http;

public class HttpClientOptionsBuilder(HttpClientOptions? options = null) {
    protected HttpClientOptions Options { get; } = options ?? new HttpClientOptions();

    public HttpClientOptionsBuilder SetBaseAddress(Uri baseAddress) {
        Options.BaseAddress = baseAddress;
        return this;
    }

    public HttpClientOptionsBuilder SetResponseFormat(string responseFormat) {
        Options.ResponseFormat = IsNotNullOrWhiteSpace(responseFormat);
        return this;
    }

    public HttpClientOptionsBuilder AddCustomHeader(string key, string value) {
        if (Options.CustomHeaders.TryGetValue(key, out var values)) {
            if (values.Contains(value)) return this;
            Options.CustomHeaders[key] = [.. values, value];
            return this;
        }
        Options.CustomHeaders[key] = [value];
        return this;
    }

    public HttpClientOptionsBuilder UseApiKeyAuthentication(string apiKey)
        => UseApiKeyAuthentication(options => options.ApiKey = apiKey);

    public HttpClientOptionsBuilder UseApiKeyAuthentication(Action<ApiKeyAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder UseSimpleTokenAuthentication(Action<StaticTokenAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder UseJsonWebTokenAuthentication(Action<JwtAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder UseOAuth2TokenAuthentication(Action<OAuth2TokenAuthenticationOptions> options, IMsalHttpClientFactory identityClientFactory)
        => SetAuthentication(options, IsNotNull(identityClientFactory));

    public HttpClientOptions Build() => IsValid(Options);

    private HttpClientOptionsBuilder SetAuthentication<T>(Action<T> configAuthentication, IMsalHttpClientFactory? identityClientFactory = null)
        where T : AuthenticationOptions, new() {
        Options.Authentication ??= new T();
        configAuthentication((T)Options.Authentication);
        if (Options.Authentication is OAuth2TokenAuthenticationOptions oAuth2Options)
            oAuth2Options.HttpClientFactory = identityClientFactory;
        return this;
    }
}
