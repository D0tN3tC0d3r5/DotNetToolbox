namespace DotNetToolbox.Http;

public class HttpClientProvider<TOptionsBuilder, TOptions>(IHttpClientFactory clientFactory, IOptions<TOptions> options)
    : IHttpClientProvider<TOptionsBuilder, TOptions>
    where TOptionsBuilder : HttpClientOptionsBuilder<TOptions>
    where TOptions : HttpClientOptions<TOptions>, new() {

    private HttpAuthentication _authentication = new();

    protected TOptions Options { get; } = IsNotNull(options).Value;

    public void RevokeAuthentication() => _authentication = new();

    public HttpClient GetHttpClient(Action<TOptionsBuilder>? configureBuilder = null) {
        var builder = Create.Instance<TOptionsBuilder>(Options);
        configureBuilder?.Invoke(builder);
        return CreateHttpClient(builder.Build());
    }

    protected HttpClient CreateHttpClient(TOptions options) {
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

public class HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
    : HttpClientProvider<HttpClientOptionsBuilder, HttpClientOptions>(clientFactory, options),
      IHttpClientProvider {
    public HttpClient GetHttpClient(string name, Action<HttpClientOptionsBuilder>? configureBuilder = null) {
        var builder = Create.Instance<HttpClientOptionsBuilder>(Options);
        configureBuilder?.Invoke(builder);
        return CreateHttpClient(builder.Build(name));
    }
}
