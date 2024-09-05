namespace DotNetToolbox.Http;

public class HttpClientProviderAccessor(IServiceProvider services)
    : IHttpClientProviderAccessor {
    public virtual IHttpClientProvider Get(string? provider = null)
        => provider is null
               ? services.GetRequiredService<IHttpClientProvider>()
               : services.GetRequiredKeyedService<IHttpClientProvider>(provider);
}
