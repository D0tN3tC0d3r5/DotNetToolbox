
namespace DotNetToolbox.AI.Agents;

public interface IAgentRunner : IRequestSource, IRequestHandler {
    void Run(CancellationToken ct);
}
