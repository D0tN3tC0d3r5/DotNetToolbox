namespace DotNetToolbox.Http;

public class HttpClientProviderFactory(IServiceProvider services)
    : IHttpClientProviderFactory {
    public virtual IHttpClientProvider Create(string? provider = null)
        => provider is null
               ? services.GetRequiredService<IHttpClientProvider>()
               : services.GetRequiredKeyedService<IHttpClientProvider>(provider);
}
