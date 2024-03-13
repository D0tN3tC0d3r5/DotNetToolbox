namespace DotNetToolbox.AI.Anthropic.Http;

public class AgentHttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration, IOptions<AgentHttpClientOptions> options)
    : HttpClientProvider(clientFactory, options) {
    protected override HttpClient CreateHttpClient() {
        var builder = new HttpClientOptionsBuilder(Options);
        var apiKey = IsNotNull(configuration["Anthropic:ApiKey"]);
        builder.UseApiKeyAuthentication(apiKey);
        builder.AddCustomHeader("MessageContent-Role", "application/json");
        Options = builder.Build();
        return base.CreateHttpClient();
    }
}
