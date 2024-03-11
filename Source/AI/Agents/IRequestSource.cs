namespace DotNetToolbox.AI.Agents;

public interface IRequestSource {
    Task ReceiveResponse(string chatId, Message response, CancellationToken ct);
}
