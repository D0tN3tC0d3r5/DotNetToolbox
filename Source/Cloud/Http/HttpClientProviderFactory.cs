namespace DotNetToolbox.Http;

public class HttpClientProviderFactory(IHttpClientFactory clientFactory, IOptions<HttpClientOptions> options)
    : IHttpClientProviderFactory {
    public IHttpClientProvider Create(string key)
        => new HttpClientProvider(key, clientFactory, options);
}
