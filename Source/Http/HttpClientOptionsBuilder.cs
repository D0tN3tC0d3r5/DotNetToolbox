namespace DotNetToolbox.Http;

public class HttpClientOptionsBuilder(HttpClientOptions? options = null)
    : HttpClientOptionsBuilder<HttpClientOptions>(options) {
    internal HttpClientOptions Build(string? name) {
        if (name is null) return IsValid(Options);
        if (!Options.Clients.TryGetValue(name, out var clientOptions))
            throw new ArgumentException("Client '{name}' not found.", nameof(name));
        clientOptions.BaseAddress ??= Options.BaseAddress;
        clientOptions.Authentication ??= Options.Authentication;
        return IsValid(clientOptions);
    }
}

public class HttpClientOptionsBuilder<TOptions>(TOptions? options = null)
    where TOptions : HttpClientOptions<TOptions>, new() {

    protected TOptions Options { get; } = options ?? new TOptions();
    //private string? _name;

    public HttpClientOptionsBuilder<TOptions> SetBaseAddress(Uri baseAddress) {
        Options.BaseAddress = baseAddress;
        return this;
    }

    public HttpClientOptionsBuilder<TOptions> SetResponseFormat(string responseFormat) {
        Options.ResponseFormat = IsNotNullOrWhiteSpace(responseFormat);
        return this;
    }

    public HttpClientOptionsBuilder<TOptions> AddCustomHeader(string key, string value) {
        if (Options.CustomHeaders.TryGetValue(key, out var values)) {
            if (values.Contains(value)) return this;
            Options.CustomHeaders[key] = [.. values, value];
            return this;
        }
        Options.CustomHeaders[key] = [value];
        return this;
    }

    public HttpClientOptionsBuilder<TOptions> UseApiKeyAuthentication(Action<ApiKeyAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder<TOptions> UseSimpleTokenAuthentication(Action<StaticTokenAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder<TOptions> UseJsonWebTokenAuthentication(Action<JwtAuthenticationOptions> options)
        => SetAuthentication(options);

    public HttpClientOptionsBuilder<TOptions> UseOAuth2TokenAuthentication(Action<OAuth2TokenAuthenticationOptions> options, IMsalHttpClientFactory identityClientFactory)
        => SetAuthentication(options, IsNotNull(identityClientFactory));

    internal TOptions Build()
        => IsValid(Options);

    private HttpClientOptionsBuilder<TOptions> SetAuthentication<T>(Action<T> configAuthentication, IMsalHttpClientFactory? identityClientFactory = null)
        where T : AuthenticationOptions, new() {
        Options.Authentication ??= new T();
        configAuthentication((T)Options.Authentication);
        if (Options.Authentication is OAuth2TokenAuthenticationOptions oAuth2Options)
            oAuth2Options.HttpClientFactory = identityClientFactory;
        return this;
    }
}
