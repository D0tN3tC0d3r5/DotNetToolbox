namespace DotNetToolbox.AI.Agents;

public interface IAgentFactory {
    IAgent Create(string? provider = null);
}
