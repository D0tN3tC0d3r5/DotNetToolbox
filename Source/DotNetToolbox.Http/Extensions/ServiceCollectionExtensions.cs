using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DotNetToolbox.Http.Extensions;
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddHttpClientProvider(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClient();
        services.AddOptions();
        services.Configure<HttpClientConfiguration>(configuration.GetSection(nameof(HttpClientOptions)));
        services.TryAddSingleton<IHttpClientProvider, HttpClientProvider>();
        return services;
    }
}
