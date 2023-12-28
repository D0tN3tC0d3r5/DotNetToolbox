namespace DotNetToolbox.Http;

public class HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
    : IHttpClientProvider {
    private readonly HttpClientOptions _options = IsNotNull(options).Value;

    private static HttpAuthentication _authentication = new();
    private static readonly object _lock = new();

    public static void RevokeAuthorization() {
        lock (_lock) _authentication = new();
    }

    public HttpClient GetHttpClient(Action<HttpClientOptionsBuilder>? configBuilder = null)
        => GetHttpClient(default!, configBuilder);

    public HttpClient GetHttpClient(string name, Action<HttpClientOptionsBuilder>? configBuilder = null) {
        var options = _options.Clients?[name] ?? _options;
        var builder = new HttpClientOptionsBuilder(options);
        configBuilder?.Invoke(builder);
        options = builder.Build();

        lock (_lock)
            return CreateHttpClient(options);
    }

    private HttpClient CreateHttpClient(HttpClientOptions options) {
        var client = clientFactory.CreateClient();
        client.BaseAddress = options.BaseAddress;
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(options.ResponseFormat));
        foreach ((var key, var value) in options.CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        _authentication = options.Authentication?.Configure(client, _authentication)!;

        return client;
    }
}
