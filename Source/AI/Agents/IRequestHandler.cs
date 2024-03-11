namespace DotNetToolbox.AI.Agents;

public interface IRequestHandler {
    Task ReceiveRequest(IRequestSource source, IChat chat, CancellationToken token);
}
