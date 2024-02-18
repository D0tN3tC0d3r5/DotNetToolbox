namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration, IOptions<OpenAIOptions> options)
    : HttpClientProvider(clientFactory, options) {
    protected override HttpClient CreateHttpClient() {
        var builder = new HttpClientOptionsBuilder(Options);
        builder.UseApiKeyAuthentication(opt => opt.ApiKey = configuration["OpenAIApiKey"]!);
        Options = builder.Build();
        return base.CreateHttpClient();
    }
}
