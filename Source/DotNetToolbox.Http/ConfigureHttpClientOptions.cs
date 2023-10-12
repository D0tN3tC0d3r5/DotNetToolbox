namespace DotNetToolbox.Http;

public class ConfigureHttpClientOptions : IConfigureOptions<HttpClientOptions> {
    private readonly IConfiguration _configuration;

    public ConfigureHttpClientOptions(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void Configure(HttpClientOptions options) {
        var section = _configuration.GetSection(nameof(HttpClientOptions));
        IsNotNull(section.Value, "Configuration[HttpClientOptions]");
        options.Configure(section);
    }
}