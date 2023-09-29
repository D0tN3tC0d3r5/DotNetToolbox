namespace DotNetToolbox.Http;

public class HttpClientBuilder<TOptions>
    where TOptions : HttpClientOptions
{
    private const string _apiKeyHeaderKey = "x-api-key";
    private readonly TOptions _options;
    private readonly HttpClient _client;

    public HttpClientBuilder(IHttpClientFactory clientFactory, IOptions<TOptions> options)
    {
        _options = options.Value;

        _client = clientFactory.CreateClient();
        _client.BaseAddress = new(_options.BaseAddress);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new(_options.ResponseFormat));
        foreach (var (key, value) in _options.CustomHeaders)
        {
            _client.DefaultRequestHeaders.Add(key, value);
        }
    }

    public HttpClient Build() => _client;

    public HttpClientBuilder<TOptions> UseApiKey()
    {
        if (_options is not IdentifiedHttpClientOptions options)
        {
            return this;
        }

        _client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, options.ApiKey);
        return this;
    }

    public async Task<HttpClientBuilder<TOptions>> AcquireTokenAsync(ITokenAcquirer? tokenAcquirer = null)
    {
        if (_options is not ConfidentialHttpClientOptions options)
        {
            return this;
        }

        tokenAcquirer ??= new TokenAcquirer();
        var token = await tokenAcquirer.AcquireTokenAsync(options);
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
        return this;
    }
}
