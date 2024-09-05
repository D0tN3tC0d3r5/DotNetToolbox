namespace DotNetToolbox.AI.Anthropic;

public class Anthropic(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : HttpClientProvider("Anthropic", httpClientFactory, configuration);
