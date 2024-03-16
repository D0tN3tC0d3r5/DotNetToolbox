namespace DotNetToolbox.AI.Agents;

public class RequestPackage(IConsumer source, IChat chat) {
    public IConsumer Source { get; } = source;
    public IChat Chat { get; } = chat;
}
