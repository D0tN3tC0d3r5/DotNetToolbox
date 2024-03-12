namespace DotNetToolbox.AI.Actors;

public interface IRequestSource {
    Task RespondWith(string chatId, Message response, CancellationToken ct);
}
