namespace DotNetToolbox.Http;

public interface IHttpClientProvider
    : IHttpClientProvider<HttpClientOptionsBuilder, HttpClientOptions> {
    HttpClient GetHttpClient(string name, Action<HttpClientOptionsBuilder>? configureBuilder = null);
}

public interface IHttpClientProvider<out TOptionsBuilder, TOptions>
    where TOptionsBuilder : HttpClientOptionsBuilder<TOptions>
    where TOptions : HttpClientOptions<TOptions>, new() {
    HttpClient GetHttpClient(Action<TOptionsBuilder>? build = null);
    void RevokeAuthentication();
}
