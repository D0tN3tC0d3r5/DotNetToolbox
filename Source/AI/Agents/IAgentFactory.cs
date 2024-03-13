namespace DotNetToolbox.AI.Agents;

public interface IAgentFactory {
    TAgent CreateAgent<TAgent>(IAgentOptions options, IPersona persona)
        where TAgent : class, IAgent;
}
