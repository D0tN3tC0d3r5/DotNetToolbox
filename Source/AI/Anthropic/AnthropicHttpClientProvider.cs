namespace DotNetToolbox.AI.Anthropic;

public class AnthropicHttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration, IOptions<AnthropicHttpClientOptions> options)
    : HttpClientProvider(clientFactory, options) {
    protected override HttpClient CreateHttpClient() {
        var builder = new HttpClientOptionsBuilder(Options);
        var apiKey = IsNotNull(configuration["Anthropic:ApiKey"]);
        builder.UseApiKeyAuthentication(apiKey);
        builder.AddCustomHeader("Content-Role", "application/json");
        Options = builder.Build();
        return base.CreateHttpClient();
    }
}
