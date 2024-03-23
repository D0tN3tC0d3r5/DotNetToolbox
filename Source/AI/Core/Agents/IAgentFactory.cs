namespace DotNetToolbox.AI.Agents;

public interface IStandardAgentFactory {
    IStandardAgent Create(string provider, World world, IAgentOptions options, Persona persona);
}
public interface IBackgroundAgentFactory {
    IBackgroundAgent Create(string provider, World world, IAgentOptions options, Persona persona);
}
public interface IQueuedAgentFactory {
    IQueuedAgent Create(string provider, World world, IAgentOptions options, Persona persona);
}
