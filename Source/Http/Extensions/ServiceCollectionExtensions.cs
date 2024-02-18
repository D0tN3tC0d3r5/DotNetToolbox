namespace DotNetToolbox.Http.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddHttpClientProvider(this IServiceCollection services, IConfiguration configuration)
        => services.AddHttpClientProvider<IHttpClientProvider, HttpClientProvider>(configuration);

    public static IServiceCollection AddHttpClientProvider<TProviderInterface, TProvider>(this IServiceCollection services, IConfiguration configuration)
        where TProviderInterface : class, IHttpClientProvider
        where TProvider : class, TProviderInterface {
        services.AddHttpClient();
        services.AddOptions();
        services.Configure<HttpClientOptions>(configuration.GetSection(HttpClientOptions.SectionName));
        services.TryAddSingleton<TProviderInterface, TProvider>();
        return services;
    }
}
