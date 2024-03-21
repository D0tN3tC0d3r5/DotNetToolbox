namespace DotNetToolbox.Http;

public class HttpClientProvider(string name, IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
    : IHttpClientProvider {
    private HttpAuthentication _authentication = new();

    public HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
        : this(string.Empty, clientFactory, options) {
    }

    protected HttpClientOptions Options { get; set; } = IsNotNull(options).Value;

    public void RevokeAuthentication() => _authentication = new();

    public HttpClient GetHttpClient(Action<HttpClientOptionsBuilder>? configureBuilder = null) {
        var builder = CreateInstance.Of<HttpClientOptionsBuilder>(Options);
        configureBuilder?.Invoke(builder);
        Options = builder.Build();
        return CreateHttpClient();
    }

    protected virtual HttpClient CreateHttpClient() {
        var client = clientFactory.CreateClient(name);
        client.BaseAddress = Options.BaseAddress;
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(Options.ResponseFormat));
        foreach ((var key, var value) in Options.CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        _authentication = Options.Authentication?.Configure(client, _authentication)!;

        return client;
    }
}
