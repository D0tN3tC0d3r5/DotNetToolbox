namespace DotNetToolbox.AI.Agents;

public class AgentAccessor(IServiceProvider services)
    : IAgentAccessor {
    public IAgent GetFor(string provider)
        => services.GetRequiredKeyedService<IAgent>(IsNotNull(provider));
}
