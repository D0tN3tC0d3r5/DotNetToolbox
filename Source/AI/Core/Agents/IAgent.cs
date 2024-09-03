namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentSettings Settings { get; }
    Persona Persona { get; set; }
    List<Tool> Tools { get; }
    Task<HttpResult> SendRequest(IJob job, IChat chat, CancellationToken ct = default);
}
