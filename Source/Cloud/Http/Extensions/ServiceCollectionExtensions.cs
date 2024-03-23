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

    public static IServiceCollection AddHttpClientProviderFactory(this IServiceCollection services, IConfiguration configuration)
        => services.AddHttpClientProviderFactory<IHttpClientProviderFactory, HttpClientProviderFactory>(configuration);

    public static IServiceCollection AddHttpClientProviderFactory<TFactoryInterface, TFactory>(this IServiceCollection services, IConfiguration configuration)
        where TFactoryInterface : class, IHttpClientProviderFactory
        where TFactory : class, TFactoryInterface {
        services.AddHttpClient();
        services.AddOptions();
        services.Configure<HttpClientOptions>(configuration.GetSection(HttpClientOptions.SectionName));
        services.TryAddSingleton<TFactoryInterface, TFactory>();
        return services;
    }

    public static IServiceCollection AddKeyedHttpClientProvider(this IServiceCollection services, string key, IConfiguration configuration)
        => services.AddKeyedHttpClientProvider<IHttpClientProvider, HttpClientProvider>(key, configuration);

    public static IServiceCollection AddKeyedHttpClientProvider<TProviderInterface, TProvider>(this IServiceCollection services, string key, IConfiguration configuration)
        where TProviderInterface : class, IHttpClientProvider
        where TProvider : class, TProviderInterface {
        services.AddHttpClient(key);
        services.AddOptions();
        services.Configure<HttpClientOptions>(configuration.GetSection($"{HttpClientOptions.SectionName}::{key}"));
        services.TryAddKeyedSingleton<TProviderInterface, TProvider>(key);
        return services;
    }

    public static IServiceCollection AddKeyedHttpClientProviderFactory(this IServiceCollection services, string key, IConfiguration configuration)
        => services.AddKeyedHttpClientProviderFactory<IHttpClientProviderFactory, HttpClientProviderFactory>(key, configuration);

    public static IServiceCollection AddKeyedHttpClientProviderFactory<TFactoryInterface, TFactory>(this IServiceCollection services, string key, IConfiguration configuration)
        where TFactoryInterface : class, IHttpClientProviderFactory
        where TFactory : class, TFactoryInterface {
        services.AddHttpClient(key);
        services.AddOptions();
        services.Configure<HttpClientOptions>(configuration.GetSection($"{HttpClientOptions.SectionName}::{key}"));
        services.TryAddKeyedSingleton<TFactoryInterface, TFactory>(key);
        return services;
    }
}
