namespace DotNetToolbox.AI.Actors;

public class RequestPackage(IRequestSource source, IChat chat) {
    public IRequestSource Source { get; } = source;
    public IChat Chat { get; } = chat;
}
