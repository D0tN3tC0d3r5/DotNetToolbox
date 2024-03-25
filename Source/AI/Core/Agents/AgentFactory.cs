namespace DotNetToolbox.AI.Agents;

public class AgentFactory(IServiceProvider services)
    : IAgentFactory {
    public TAgent Create<TAgent>(string provider)
        where TAgent : class, IAgent
        => (TAgent)services.GetRequiredKeyedService<IAgent>(provider);
}
