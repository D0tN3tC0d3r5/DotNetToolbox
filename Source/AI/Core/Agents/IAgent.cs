namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentSettings Settings { get; }
    Task<HttpResult> SendRequest(IMessages messages, JobContext jobContext, CancellationToken ct = default);
}
