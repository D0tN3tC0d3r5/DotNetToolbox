
namespace DotNetToolbox.AI.Agents;

public interface IAgentRunner : IOriginator, IRequestHandler {
    Task Start(CancellationToken ct);
}
