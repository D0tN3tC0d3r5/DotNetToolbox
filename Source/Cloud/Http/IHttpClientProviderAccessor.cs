namespace DotNetToolbox.Http;

public interface IHttpClientProviderAccessor {
    IHttpClientProvider Get(string? provider = null);
}
