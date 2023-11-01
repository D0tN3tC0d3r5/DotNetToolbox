namespace DotNetToolbox.Http.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddHttpClientProvider(this IServiceCollection services, IConfiguration configuration) {
        _ = services.AddHttpClient();
        _ = services.AddOptions();
        _ = services.Configure<HttpClientConfiguration>(configuration.GetSection(nameof(HttpClientOptions)));
        services.TryAddSingleton<IHttpClientProvider, HttpClientProvider>();
        return services;
    }
}
