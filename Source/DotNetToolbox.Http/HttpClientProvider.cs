namespace DotNetToolbox.Http;

public class HttpClientProvider : IHttpClientProvider {
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClientConfiguration _config;
    private readonly IMsalHttpClientFactory _identityClientFactory;

    private static HttpAuthentication _authentication = new();
    private static readonly object _lock = new();

    public HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientConfiguration> options, IMsalHttpClientFactory identityClientFactory) {
        _clientFactory = clientFactory;
        _config = IsNotNull(options).Value;
        _identityClientFactory = IsNotNull(identityClientFactory);
    }
    public static void RevokeAuthorization() {
        lock (_lock) _authentication = new();
    }

    public HttpClient GetHttpClient(Action<IHttpClientOptionsBuilder>? configBuilder = null)
        => GetHttpClient(default!, configBuilder);

    public HttpClient GetHttpClient(string name, Action<IHttpClientOptionsBuilder>? configBuilder = null) {
        var builder = new HttpClientOptionsBuilder(name, _config, _identityClientFactory);
        configBuilder?.Invoke(builder);
        var options = builder.Build();
        options.Validate().EnsureIsValid($"'{name ?? "Default"}' http client options are not valid.");

        var client = _clientFactory.CreateClient();
        lock (_lock) options.Configure(client, ref _authentication);
        return client;
    }

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateWithAuthorizationCodeAsync(string authorizationCode)
    //    => CreateApplication()
    //    .AcquireTokenByAuthorizationCode(_config.Authentication.Scopes, authorizationCode)
    //    .ExecuteAsync(CancellationToken.None);

    //[ExcludeFromCodeCoverage]
    //private Task<AuthenticationResult> AuthenticateAccountAsync(IAccount account)
    //    => CreateApplication()
    //    .AcquireTokenSilent(_config.Authentication.Scopes, account)
    //    .ExecuteAsync(CancellationToken.None);
}
