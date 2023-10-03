namespace DotNetToolbox.Http;

public class HttpClientBuilder {
    private const string _apiKeyHeaderKey = "x-api-key";
    private readonly HttpClientOptions _options;
    private readonly HttpClient _client;

    public HttpClientBuilder(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options) {
        _options = options.Value;

        _client = clientFactory.CreateClient();
        _client.BaseAddress = new(_options.BaseAddress);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new(_options.ResponseFormat));
        foreach ((var key, var value) in _options.CustomHeaders) {
            _client.DefaultRequestHeaders.Add(key, value);
        }
    }

    public HttpClient Build() => _client;

    public HttpClientBuilder UseApiKey() {
        _client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, IsNotNullOrWhiteSpace(_options.ApiKey));
        return this;
    }

    public async Task<HttpClientBuilder> AcquireTokenAsync(ITokenAcquirer? tokenAcquirer = null) {
        var app = ConfidentialClientApplicationBuilder
                 .Create(IsNotNullOrWhiteSpace(_options.ClientId))
                 .WithAuthority(IsNotNullOrWhiteSpace(_options.Authority))
                 .WithClientSecret(IsNotNullOrWhiteSpace(_options.ClientSecret))
                 .Build();

        var builder = app.AcquireTokenForClient(_options.Scopes);
        tokenAcquirer ??= new TokenAcquirer<AcquireTokenForClientParameterBuilder>(builder);
        var token = await tokenAcquirer.AcquireTokenAsync();
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
        return this;
    }
}
