namespace DotNetToolbox.Http;

public class HttpClientProvider {
    private readonly IHttpClientFactory _clientFactory;
    private readonly IMsalHttpClientFactory _identityClientFactory;
    private readonly HttpClientOptions _options;

    private static HttpClientAuthentication _authentication = new();
    private static readonly object _lock = new();

    public HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options, IMsalHttpClientFactory identityClientFactory) {
        _clientFactory = clientFactory;
        _identityClientFactory = identityClientFactory;
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract - False positive
        _options = options.Value;
    }
    public static void RevokeAuthorization() {
        lock (_lock) _authentication = new();
    }

    public HttpClientProvider UseApiKey(Action<ApiKeyAuthenticationOptions> configAuthentication)
        => SetAuthentication(configAuthentication);

    public HttpClientProvider UseSimpleToken(Action<StaticTokenAuthenticationOptions> configAuthentication)
        => SetAuthentication(configAuthentication);

    public HttpClientProvider UseJsonWebToken(Action<JwtAuthenticationOptions> configAuthentication)
        => SetAuthentication(configAuthentication);

    public HttpClientProvider UseOAuth2Token(Action<OAuth2TokenAuthenticationOptions> configAuthentication)
        => SetAuthentication(configAuthentication);

    public HttpClient GetHttpClient(string? name = null) {
        var options = _options.ResolveOptionsFor(name);
        options.Validate(name).EnsureIsValid();

        var client = _clientFactory.CreateClient();
        lock (_lock) options.Configure(client, ref _authentication);
        return client;
    }

    private HttpClientProvider SetAuthentication<T>(Action<T> configAuthentication)
        where T : AuthenticationOptions, new() {
        _options.Authentication ??= new T();
        configAuthentication((T)_options.Authentication);
        if (_options.Authentication is OAuth2TokenAuthenticationOptions oAuth2Options)
            oAuth2Options.HttpClientFactory = _identityClientFactory;
        return this;
    }

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateWithAuthorizationCodeAsync(string authorizationCode)
    //    => CreateApplication()
    //    .AcquireTokenByAuthorizationCode(_options.Authentication.Scopes, authorizationCode)
    //    .ExecuteAsync(CancellationToken.None);

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateAccountAsync(IAccount account)
    //    => CreateApplication()
    //    .AcquireTokenSilent(_options.Authentication.Scopes, account)
    //    .ExecuteAsync(CancellationToken.None);
}
