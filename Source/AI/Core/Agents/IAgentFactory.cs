namespace DotNetToolbox.AI.Agents;

public interface IAgentFactory {
    TAgent Create<TAgent>(string provider)
        where TAgent : class, IAgent;
}
