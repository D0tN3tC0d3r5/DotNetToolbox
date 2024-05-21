namespace DotNetToolbox.Http.Extensions;

public class HttpClientProviders {
    private readonly List<IHttpClientProvider> _providers = [];

    public void RegisterProvider(IHttpClientProvider provider)
        => _providers.Add(provider);

    public IReadOnlyList<IHttpClientProvider> GetProviderList()
        => _providers;

    public IHttpClientProvider? GetProviderOrDefault(string name)
        => _providers.Find(p => p.Name == name);
}
