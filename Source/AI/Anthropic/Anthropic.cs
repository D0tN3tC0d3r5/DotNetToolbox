namespace DotNetToolbox.AI.Anthropic;

public class Anthropic(IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider("Anthropic", clientFactory, configuration) {

    protected override void SetDefaultConfiguration(HttpClientOptions options) {
        options.CustomHeaders ??= new();
        options.CustomHeaders.Add("MessageContent-Role", ["application/json"]);
    }
}
