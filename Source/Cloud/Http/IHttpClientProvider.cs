namespace DotNetToolbox.Http;

public interface IHttpClientProvider {
    HttpClient GetHttpClient(string? name = null, Action<HttpClientOptionsBuilder>? configureBuilder = null);
    void RevokeAuthentication();
}
