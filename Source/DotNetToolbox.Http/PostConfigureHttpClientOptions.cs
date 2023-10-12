namespace DotNetToolbox.Http;

public class PostConfigureHttpClientOptions : IPostConfigureOptions<HttpClientConfiguration> {
    public void PostConfigure(string? name, HttpClientConfiguration config)
        => throw new NotImplementedException();
}