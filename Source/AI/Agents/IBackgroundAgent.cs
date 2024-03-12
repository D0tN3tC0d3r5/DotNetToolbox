namespace DotNetToolbox.AI.Agents;

public interface IBackgroundAgent {
    void Run(CancellationToken ct);
}
