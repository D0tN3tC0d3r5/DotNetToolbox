namespace DotNetToolbox.AI.Agent;

public interface IAgent {
    string Id { get; }
    Task Start(CancellationToken ct);
    CancellationTokenSource AddRequest(IAgent source, IChat chat);
    void AddResponse(Package request);
}
