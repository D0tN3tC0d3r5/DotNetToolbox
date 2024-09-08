namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentSettings Settings { get; }
    Task<HttpResult> SendRequest(IChat chat, JobContext context, CancellationToken ct = default);
}
