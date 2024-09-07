namespace DotNetToolbox.AI.Agents;

public interface IAgentAccessor {
    IAgent GetFor(string provider);
}
