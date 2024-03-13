namespace DotNetToolbox.AI.Agents;

public interface IAgentFactory {
    TAgent CreateAgent<TAgent>(IAgentOptions options, Persona persona)
        where TAgent : class, IAgent;
}
