namespace DotNetToolbox.AI.Agent;

public class Package(IAgent agent, IChat chat, CancellationToken token) {
    public string Id { get; } = Guid.NewGuid().ToString();
    public IAgent Agent { get; } = agent;
    public IChat Chat { get; } = chat;
    public CancellationToken Token { get; } = token;
}
