namespace DotNetToolbox.AI.OpenAI;

public class OpenAI(IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider("OpenAI", clientFactory, configuration) {
    private readonly IConfiguration _configuration = configuration;

    protected override void SetDefaultConfiguration(HttpClientOptions options) {
        options.CustomHeaders ??= new();
        var organization = IsNotNull(_configuration["HttpClient:OpenAI:Organization"]);
        options.CustomHeaders.Add("OpenAI-Organization", [organization]);
    }
}
