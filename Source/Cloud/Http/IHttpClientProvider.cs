namespace DotNetToolbox.Http;

public interface IHttpClientProvider {
    HttpClient GetHttpClient(Action<HttpClientOptionsBuilder>? configureBuilder = null);
    void RevokeAuthentication();
}
