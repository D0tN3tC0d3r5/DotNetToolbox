namespace DotNetToolbox.AI.Agents;

public class AgentFactory(IServiceProvider services)
    : IAgentFactory {
    public IAgent Create(string provider)
        => services.GetRequiredKeyedService<IAgent>(provider);
}
