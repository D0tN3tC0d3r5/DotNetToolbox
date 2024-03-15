using DotNetToolbox.AI.Anthropic.Http;

namespace DotNetToolbox.AI.Anthropic;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddAnthropic(this IServiceCollection services, IConfiguration configuration) {
        services.AddAI<AgentHttpClientProvider, AgentFactory>(configuration);
        return services;
    }
}
