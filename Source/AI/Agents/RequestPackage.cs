namespace DotNetToolbox.AI.Agents;

public class RequestPackage(IRequestSource source, IChat chat, CancellationToken token) {
    public IRequestSource Source { get; } = source;
    public IChat Chat { get; } = chat;
    public CancellationToken Token { get; } = token;
}
