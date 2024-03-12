namespace DotNetToolbox.AI.Actors;

public interface IRequestHandler {
    Task RespondTo(IRequestSource source, IChat chat, CancellationToken token);
}
