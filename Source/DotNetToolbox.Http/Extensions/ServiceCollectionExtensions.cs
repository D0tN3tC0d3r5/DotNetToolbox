
namespace DotNetToolbox.Http.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddHttpClientProvider(this IServiceCollection services, IConfiguration configuration) {
        services.TryAddSingleton<IMsalHttpClientFactory, NullMsalHttpClientFactory>();
        services.AddHttpClient();
        services.AddOptions();
        services.Configure<HttpClientConfiguration>(configuration.GetSection(nameof(HttpClientOptions)));
        services.TryAddSingleton<IHttpClientProvider, HttpClientProvider>();
        return services;
    }

    public class NullMsalHttpClientFactory : IMsalHttpClientFactory {
        public HttpClient GetHttpClient() => throw new NotImplementedException();
    }
}
