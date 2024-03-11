
namespace DotNetToolbox.AI.Agents;

public interface IAgentRunner : IOriginator, IRequestHandler {
    void Run(CancellationToken ct);
}
