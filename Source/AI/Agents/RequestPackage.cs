namespace DotNetToolbox.AI.Agents;

public class RequestPackage(IOriginator source, IChat chat, CancellationToken token) {
    public IOriginator Source { get; } = source;
    public IChat Chat { get; } = chat;
    public CancellationToken Token { get; } = token;
}
