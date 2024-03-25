namespace DotNetToolbox.AI.Agents;

public interface IBackgroundAgent : IAgent {
    void Run(CancellationToken ct);
}
