namespace DotNetToolbox.AI.Agents;

public class AgentFactory(IServiceProvider services, IConfiguration configuration)
    : IAgentFactory {
    public IAgent Create(string? provider = null) {
        provider ??= configuration["AI:DefaultProvider"];
        return services.GetRequiredKeyedService<IAgent>(IsNotNull(provider));
    }
}
