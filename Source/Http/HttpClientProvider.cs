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
        var options = builder.Build();
        options.Validate().EnsureIsValid();

        var client = clientFactory.CreateClient();
        lock (_lock) options.Configure(client, ref _authentication);
        return client;
    }
}
