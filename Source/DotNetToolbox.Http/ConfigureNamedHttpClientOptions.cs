namespace DotNetToolbox.Http;

public class ConfigureNamedHttpClientOptions : IConfigureNamedOptions<HttpClientOptions> {
    public void Configure(string? name, HttpClientOptions options) {
    }

    public void Configure(HttpClientOptions options)
        => Configure(nameof(HttpClientOptions), options);
}