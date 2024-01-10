namespace DotNetToolbox.Http.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddHttpClientProvider(this IServiceCollection services, IConfiguration configuration)
        => services.AddHttpClientProvider<IHttpClientProvider, HttpClientProvider>(configuration);

    public static IServiceCollection AddHttpClientProvider<TProviderInterface, TProvider>(this IServiceCollection services, IConfiguration configuration)
        where TProviderInterface : class, IHttpClientProvider<HttpClientOptionsBuilder, HttpClientOptions>
        where TProvider : class, TProviderInterface
        => services.AddHttpClientProvider<TProviderInterface, TProvider, HttpClientOptionsBuilder, HttpClientOptions>(configuration);

    public static IServiceCollection AddHttpClientProvider<TProviderInterface, TProvider, TOptionsBuilder, TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TProviderInterface : class, IHttpClientProvider<TOptionsBuilder, TOptions>
        where TProvider : class, TProviderInterface
        where TOptionsBuilder : HttpClientOptionsBuilder<TOptions>
        where TOptions : HttpClientOptions<TOptions>, INamedOptions<TOptions>, new() {
        services.AddHttpClient();
        services.AddOptions();
        services.Configure<TOptions>(configuration.GetSection(TOptions.SectionName));
        services.TryAddSingleton<TProviderInterface, TProvider>();
        return services;
    }
}
