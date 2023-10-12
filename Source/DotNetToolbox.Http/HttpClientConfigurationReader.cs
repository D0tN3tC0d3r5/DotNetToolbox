namespace DotNetToolbox.Http;

public class HttpClientConfigurationReader : IConfigureOptions<HttpClientConfiguration> {
    private readonly IConfiguration _configuration;

    public HttpClientConfigurationReader(IConfiguration configuration) {
        _configuration = configuration;
    }

    public void Configure(HttpClientConfiguration config) {
        var section = _configuration.GetSection(nameof(HttpClientConfiguration));
        IsNotNull(section.Value, "Configuration[HttpClientConfiguration]");
        config.Read(section);
    }
}