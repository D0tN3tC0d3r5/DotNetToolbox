namespace DotNetToolbox.AI.Anthropic;

public class Anthropic(IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider("Anthropic", clientFactory, configuration) {
    private readonly IConfiguration _configuration = configuration;

    protected override void Configure(HttpClientOptionsBuilder builder) {
        var apiKey = IsNotNull(_configuration["HttpClient:Anthropic:ApiKey"]);
        builder.UseApiKeyAuthentication(apiKey);
        builder.AddCustomHeader("MessageContent-Role", "application/json");
    }
}
