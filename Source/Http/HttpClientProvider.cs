namespace DotNetToolbox.Http;

//public class HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
//    : HttpClientProvider<HttpClientOptionsBuilder, HttpClientOptions>(clientFactory, options),
//      IHttpClientProvider {
//    public HttpClient GetHttpClient(string name, Action<HttpClientOptionsBuilder>? configureBuilder = null) {
//        var builder = CreateInstance.Of<HttpClientOptionsBuilder>(Options);
//        configureBuilder?.Invoke(builder);
//        return CreateHttpClient(builder.Build(name));
//    }
//}

public class HttpClientProvider(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
    : IHttpClientProvider {
    private HttpAuthentication _authentication = new();

    protected HttpClientOptions Options { get; set; } = IsNotNull(options).Value;

    public void RevokeAuthentication() => _authentication = new();

    public HttpClient GetHttpClient(string? name = null, Action<HttpClientOptionsBuilder>? configureBuilder = null) {
        var builder = CreateInstance.Of<HttpClientOptionsBuilder>(Options);
        configureBuilder?.Invoke(builder);
        Options = builder.Build(name);
        return CreateHttpClient();
    }

    protected virtual HttpClient CreateHttpClient() {
        var client = clientFactory.CreateClient();
        client.BaseAddress = Options.BaseAddress;
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(Options.ResponseFormat));
        foreach ((var key, var value) in Options.CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        _authentication = Options.Authentication?.Configure(client, _authentication)!;

        return client;
    }
}
