namespace DotNetToolbox.AI.OpenAI;

public class OpenAI(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : HttpClientProvider("OpenAI", httpClientFactory, configuration);
