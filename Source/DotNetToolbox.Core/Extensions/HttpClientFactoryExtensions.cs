namespace DotNetToolbox.Extensions;

public interface ITokenAcquirer
{
    Task<string> AcquireTokenAsync<TOptions>(TOptions options)
        where TOptions : ConfidentialHttpClientOptions;
}

[ExcludeFromCodeCoverage]
internal class TokenAcquirer
    : ITokenAcquirer
{
    public async Task<string> AcquireTokenAsync<TOptions>(TOptions options)
        where TOptions : ConfidentialHttpClientOptions
    {
        var app = ConfidentialClientApplicationBuilder
                  .Create(options.ClientId)
                  .WithAuthority(options.Authority)
                  .WithClientSecret(options.ClientSecret)
                  .Build();

        var acquireToken = app.AcquireTokenForClient(options.Scopes);
        var token = await acquireToken.ExecuteAsync(CancellationToken.None);
        return token.CreateAuthorizationHeader();
    }
}

public static class HttpClientFactoryExtensions
{
    private const string _apiKeyHeaderKey = "x-api-key";

    public static HttpClient CreateIdentifiedHttpClient<TOptions>(this IHttpClientFactory clientFactory, IOptions<TOptions> options)
        where TOptions : IdentifiedHttpClientOptions
    {
        var client = clientFactory.CreateHttpClient(options);
        client.DefaultRequestHeaders.Add(_apiKeyHeaderKey, options.Value.ApiKey);

        return client;
    }

    public static async Task<HttpClient> CreateConfidentialHttpClientAsync<TOptions>(this IHttpClientFactory clientFactory, IOptions<TOptions> options)
        where TOptions : ConfidentialHttpClientOptions
    {
        var client = clientFactory.CreateHttpClient(options);
        var token = await TokenAcquirer.AcquireTokenAsync(options.Value);
        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);

        return client;
    }

    private static ITokenAcquirer? _tokenAcquirer;
    [ExcludeFromCodeCoverage]
    internal static ITokenAcquirer TokenAcquirer
    {
        get => _tokenAcquirer ??= new TokenAcquirer();
        set => _tokenAcquirer = value;
    }

    private static HttpClient CreateHttpClient<TOptions>(this IHttpClientFactory clientFactory, IOptions<TOptions> options)
        where TOptions : HttpClientOptions
    {
        var optionsValue = options.Value;
        var client = clientFactory.CreateClient();
        client.BaseAddress = new(optionsValue.BaseAddress);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(optionsValue.ResponseFormat));
        client.AddCustomHeaders(optionsValue);

        return client;
    }

    private static void AddCustomHeaders(this HttpClient client, HttpClientOptions options)
    {
        foreach ((var key, var value) in options.CustomHeaders)
        {
            client.DefaultRequestHeaders.Add(key, value);
        }   
    }
}
