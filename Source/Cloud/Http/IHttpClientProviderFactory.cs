namespace DotNetToolbox.Http;

public interface IHttpClientProviderFactory {
    IHttpClientProvider Create(string provider);
}
