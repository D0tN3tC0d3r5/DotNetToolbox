namespace DotNetToolbox.AI.Agents;

public interface IAgentFactory {
    TAgent CreateAgent<TAgent>(World world, IAgentOptions options, Persona persona)
        where TAgent : class, IAgent;
}
