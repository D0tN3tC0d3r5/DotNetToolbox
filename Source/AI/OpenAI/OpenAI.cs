namespace DotNetToolbox.AI.OpenAI;

public class OpenAI(IHttpClientFactory clientFactory, IConfiguration configuration)
    : HttpClientProvider("OpenAI", clientFactory, configuration) {
    private readonly IConfiguration _configuration = configuration;

    protected override void Configure(HttpClientOptionsBuilder builder) {
        var key = IsNotNull(_configuration["HttpClient:OpenAI:ApiKey"]);
        var organization = IsNotNull(_configuration["HttpClient:OpenAI:Organization"]);
        builder.UseTokenAuthentication(opt => {
            opt.Scheme = AuthenticationScheme.Bearer;
            opt.Token = key;
        });
        builder.AddCustomHeader("OpenAI-Organization", organization);
    }
}
