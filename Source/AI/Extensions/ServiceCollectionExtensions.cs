using DotNetToolbox.AI.Actors;

namespace DotNetToolbox.AI.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, OpenAIHttpClientProvider>(configuration);
        services.TryAddScoped<IBackgroundRunner, OpenAIQueuedActor>();
        services.TryAddScoped<IModelsHandler, OpenAIModelsHandler>();
        return services;
    }

    public static IServiceCollection AddAnthropic(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, AnthropicHttpClientProvider>(configuration);
        services.TryAddScoped<IBackgroundRunner, AnthropicQueuedActor>();
        return services;
    }
}
