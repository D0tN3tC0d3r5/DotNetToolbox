namespace DotNetToolbox.AI.Agents;

public interface IBackgroundAgent : IStandardAgent {
    void Run(CancellationToken ct);
}
