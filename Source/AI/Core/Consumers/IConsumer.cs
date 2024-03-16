namespace DotNetToolbox.AI.Consumers;

public interface IConsumer {
    Task ProcessResponse(string chatId, Message response, CancellationToken ct);
}
