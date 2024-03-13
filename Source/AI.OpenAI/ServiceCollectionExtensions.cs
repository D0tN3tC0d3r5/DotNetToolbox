using DotNetToolbox.AI.OpenAI.Http;

namespace DotNetToolbox.AI.OpenAI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, AgentHttpClientProvider>(configuration);
        services.TryAddSingleton<World>();
        services.TryAddSingleton<IAgentFactory, AgentFactory>();
        return services;
    }
}
