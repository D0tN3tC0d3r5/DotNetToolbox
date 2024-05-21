namespace DotNetToolbox.AI.Agents;

public class RequestPackage(IResponseAwaiter source, IChat chat, int? number) {
    public IResponseAwaiter Source { get; } = source;
    public IChat Chat { get; } = chat;
    public int? AgentNumber { get; } = number;
}
