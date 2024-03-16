namespace DotNetToolbox.AI.OpenAI.Http;

public class AgentHttpClientProvider(IHttpClientFactory clientFactory, IConfiguration configuration, IOptions<AgentHttpClientOptions> options)
    : HttpClientProvider(clientFactory, options) {
    protected override System.Net.Http.HttpClient CreateHttpClient() {
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
