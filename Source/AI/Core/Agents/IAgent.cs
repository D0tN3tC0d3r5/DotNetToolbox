namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentSettings Settings { get; }
    Task<HttpResult<Message>> SendRequest(IChat chat, JobContext context, CancellationToken ct = default);
}
