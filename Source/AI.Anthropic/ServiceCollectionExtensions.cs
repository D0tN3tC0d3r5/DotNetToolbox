using DotNetToolbox.AI.Anthropic.Http;

namespace DotNetToolbox.AI.Anthropic;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAnthropic(this IServiceCollection services, IConfiguration configuration) {
        services.AddHttpClientProvider<IHttpClientProvider, AgentHttpClientProvider>(configuration);
        services.TryAddSingleton<IAgentFactory, AgentFactory>();
        return services;
    }
}
