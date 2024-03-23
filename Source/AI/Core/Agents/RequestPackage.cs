namespace DotNetToolbox.AI.Agents;

public class RequestPackage(IResponseAwaiter source, IChat chat) {
    public IResponseAwaiter Source { get; } = source;
    public IChat Chat { get; } = chat;
}
