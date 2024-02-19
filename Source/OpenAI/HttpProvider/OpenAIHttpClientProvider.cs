namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration, IOptions<OpenAIOptions> options)
    : HttpClientProvider(clientFactory, options) {
    protected override HttpClient CreateHttpClient() {
        var builder = new HttpClientOptionsBuilder(Options);
        var key = IsNotNull(configuration["OpenAI:ApiKey"]);
        var organization = IsNotNull(configuration["OpenAI:Organization"]);
        builder.UseSimpleTokenAuthentication(opt => {
            opt.Scheme = AuthenticationScheme.Bearer;
            opt.Token = key;
        });
        builder.AddCustomHeader("OpenAI-Organization", organization);
        Options = builder.Build();
        return base.CreateHttpClient();
    }
}
