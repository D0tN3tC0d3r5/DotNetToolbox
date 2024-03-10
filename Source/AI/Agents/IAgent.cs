
namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    string Id { get; }
    Profile Profile { get; }
    List<Skill> Skills { get; }

    Task Start(CancellationToken ct);
    CancellationTokenSource AddRequest(IAgent source, IChat chat);
    void AddResponse(Package request);
}
