using DotNetToolbox.Utilities;

namespace DotNetToolbox.Http.Extensions;

public static class ServiceCollectionExtensions {
    private static readonly HttpClientProviders _providers = new();

    public static IServiceCollection AddHttpClientProvider(this IServiceCollection services, string? provider = null)
        => services.AddHttpClientProvider<HttpClientProvider>(provider);

    public static IServiceCollection AddHttpClientProvider<TProvider>(this IServiceCollection services, string? provider = null)
        where TProvider : class, IHttpClientProvider {
        services.AddHttpClient();
        services.TryAddSingleton(_providers);
        if (string.IsNullOrWhiteSpace(provider)) {
            services.TryAddSingleton<IHttpClientProvider, TProvider>();
            return services;
        }

        services.AddKeyedSingleton<IHttpClientProvider>(provider, (sp, _) => {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var configuration = sp.GetRequiredService<IConfiguration>();
            var instance = InstanceFactory.Create<TProvider>(provider, factory, configuration);
            _providers.RegisterProvider(instance);
            return instance;
        });
        return services;
    }

    public static IServiceCollection AddHttpClientProviderFactory(this IServiceCollection services) {
        services.AddHttpClient();
        services.TryAddSingleton<IHttpClientProviderFactory, HttpClientProviderFactory>();
        return services;
    }
}
