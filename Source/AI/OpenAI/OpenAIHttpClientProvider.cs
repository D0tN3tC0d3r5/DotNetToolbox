namespace DotNetToolbox.AI.OpenAI;

public class OpenAIHttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration, IOptions<OpenAIHttpClientOptions> options)
    : HttpClientProvider(clientFactory, options) {
    protected override HttpClient CreateHttpClient() {
        var builder = new HttpClientOptionsBuilder(Options);
        var key = IsNotNull(configuration["OpenAI:ApiKey"]);
        var organization = IsNotNull(configuration["OpenAI:Organization"]);
        builder.UseApiKeyAuthentication(opt => opt.ApiKey = key);
        builder.AddCustomHeader("OpenAI-Organization", organization);
        Options = builder.Build();
        return base.CreateHttpClient();
    }
}
