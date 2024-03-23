namespace DotNetToolbox.AI.Agents;

public interface IStandardAgentFactory {
    IStandardAgent Create(string provider, World world, Persona persona, IAgentOptions options);
}
public interface IBackgroundAgentFactory {
    IBackgroundAgent Create(string provider, World world, Persona persona, IAgentOptions options);
}
public interface IQueuedAgentFactory {
    IQueuedAgent Create(string provider, World world, Persona persona, IAgentOptions options);
}
