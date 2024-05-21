namespace DotNetToolbox.AI.Anthropic;

public class Anthropic(IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider("Anthropic", clientFactory, configuration);
