namespace DotNetToolbox.Http;

public interface IHttpClientProvider {
    HttpClient GetHttpClient(Action<HttpClientOptionsBuilder>? build = null);
    HttpClient GetHttpClient(string name, Action<HttpClientOptionsBuilder>? build = null);
}
