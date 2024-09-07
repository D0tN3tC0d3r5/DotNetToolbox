namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentSettings Settings { get; }
    Task<HttpResult<string>> SendRequest(IMessages messages, JobContext jobContext, CancellationToken ct = default);
}
