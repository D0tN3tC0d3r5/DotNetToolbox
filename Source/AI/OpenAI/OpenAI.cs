namespace DotNetToolbox.AI.OpenAI;

public class OpenAI(IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider("OpenAI", clientFactory, configuration);
