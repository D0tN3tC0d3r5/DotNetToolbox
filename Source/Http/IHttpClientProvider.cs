namespace DotNetToolbox.Http;

public interface IHttpClientProvider {
    HttpClient GetHttpClient(Action<IHttpClientOptionsBuilder>? build = null);
    HttpClient GetHttpClient(string name, Action<IHttpClientOptionsBuilder>? build = null);
}