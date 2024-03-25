namespace DotNetToolbox.Http;

public class HttpClientOptionsBuilder(HttpClientOptions options) {
    private Uri? _baseAddress;
    private string _responseFormat = HttpClientOptions.DefaultResponseFormat;
    private readonly Dictionary<string, string[]> _customHeaders = [];
    private AuthenticationOptions? _authentication;

    public HttpClientOptionsBuilder SetBaseAddress(Uri baseAddress) {
        _baseAddress = baseAddress;
        return this;
    }

    public HttpClientOptionsBuilder SetResponseFormat(string responseFormat) {
        _responseFormat = IsNotNullOrWhiteSpace(responseFormat);
        return this;
    }

    public HttpClientOptionsBuilder AddCustomHeader(string key, string value) {
        if (_customHeaders.TryGetValue(key, out var values)) {
            if (values.Contains(value)) return this;
            _customHeaders[key] = [.. values, value];
            return this;
        }
        _customHeaders[key] = [value];
        return this;
    }

    public HttpClientOptionsBuilder UseApiKeyAuthentication(string apiKey, AuthenticationScheme scheme = Bearer)
        => UseApiKeyAuthentication(o => {
            o.ApiKey = apiKey;
            o.Scheme = scheme;
        });

    public HttpClientOptionsBuilder UseApiKeyAuthentication(Action<ApiKeyAuthenticationOptions> configure)
        => SetAuthentication(configure);

    public HttpClientOptionsBuilder UseTokenAuthentication(Action<TokenAuthenticationOptions> configure)
        => SetAuthentication(configure);

    public HttpClientOptionsBuilder UseJwtAuthentication(Action<JwtAuthenticationOptions> configure)
        => SetAuthentication(configure);

    public HttpClientOptionsBuilder UseOAuth2TokenAuthentication(Action<OAuth2TokenAuthenticationOptions> configure, IMsalHttpClientFactory identityClientFactory)
        => SetAuthentication(configure, IsNotNull(identityClientFactory));

    public HttpClientOptions Build() {
        options = options with {
            BaseAddress = IsNotNull(_baseAddress),
            ResponseFormat = IsNotNullOrWhiteSpace(_responseFormat),
            Authentication = _authentication,
            CustomHeaders = _customHeaders,
        };
        return IsValid(options);
    }

    private HttpClientOptionsBuilder SetAuthentication<T>(Action<T> configAuthentication, IMsalHttpClientFactory? identityClientFactory = null)
        where T : AuthenticationOptions, new() {
        _authentication = new T();
        configAuthentication((T)_authentication);
        if (_authentication is OAuth2TokenAuthenticationOptions oAuth2Options)
            oAuth2Options.HttpClientFactory = identityClientFactory;
        return this;
    }
}
