namespace DotNetToolbox.Http;

public class HttpClientProvider
    : IHttpClientProvider {
    private HttpAuthentication _authentication = new();
    private readonly string _name;
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClientOptionsBuilder _builder;

    public HttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration)
        : this(string.Empty, clientFactory, configuration) {
    }

    public HttpClientProvider(string name, IHttpClientFactory clientFactory, IConfiguration configuration) {
        _name = name;
        _clientFactory = clientFactory;
        var sectionPath = HttpClientOptions.SectionName + (string.IsNullOrWhiteSpace(_name) ? string.Empty : $":{_name}");
        var options = new HttpClientOptions();
        configuration.GetSection(sectionPath).Bind(options);
        _builder = new(IsValid(options));
        Configure(_builder);
    }

    public void RevokeAuthentication() => _authentication = new();

    public HttpClient GetHttpClient(Action<HttpClientOptionsBuilder>? configureBuilder = null) {
        configureBuilder?.Invoke(_builder);
        var options = _builder.Build();
        return CreateHttpClient(options);
    }

    protected virtual void Configure(HttpClientOptionsBuilder builder) {
    }

    private HttpClient CreateHttpClient(HttpClientOptions options) {
        var client = _clientFactory.CreateClient(_name);
        client.BaseAddress = options.BaseAddress;
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(options.ResponseFormat));
        foreach ((var key, var value) in options.CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);
        _authentication = options.Authentication?.Configure(client, _authentication)!;
        return client;
    }
}
