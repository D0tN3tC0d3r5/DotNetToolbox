namespace DotNetToolbox.Http;

public class HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientConfiguration> options, IMsalHttpClientFactory identityClientFactory)
    : IHttpClientProvider {
    private readonly HttpClientConfiguration _config = IsNotNull(options).Value;
    private readonly IMsalHttpClientFactory _identityClientFactory = IsNotNull(identityClientFactory);

    private static HttpAuthentication _authentication = new();
    private static readonly object _lock = new();

    public static void RevokeAuthorization() {
        lock (_lock) _authentication = new();
    }

    public HttpClient GetHttpClient(Action<IHttpClientOptionsBuilder>? configBuilder = null)
        => GetHttpClient(default!, configBuilder);

    public HttpClient GetHttpClient(string name, Action<IHttpClientOptionsBuilder>? configBuilder = null) {
        var builder = new HttpClientOptionsBuilder(name, _config, _identityClientFactory);
        configBuilder?.Invoke(builder);
        var clientOptions = builder.Build();
        clientOptions.Validate().EnsureIsValid();

        var client = clientFactory.CreateClient();
        lock (_lock) clientOptions.Configure(client, ref _authentication);
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
