using DotNetToolbox.AI.OpenAI.Http;

namespace DotNetToolbox.AI.OpenAI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddOpenAI(this IServiceCollection services, IConfiguration configuration) {
        services.AddAI<AgentHttpClientProvider, AgentFactory>(configuration);
        return services;
    }
}
