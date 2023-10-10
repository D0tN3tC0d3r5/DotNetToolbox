namespace DotNetToolbox.Http;

public class PostConfigureHttpClientOptions : IPostConfigureOptions<HttpClientOptions> {
    public void PostConfigure(string? name, HttpClientOptions options)
        => throw new NotImplementedException();
}