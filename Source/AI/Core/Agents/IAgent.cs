namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentModel Model { get; set; }
    World World { get; set; }
    Persona Persona { get; set; }
    Task<HttpResult> SendRequest(IJob job, IChat chat, CancellationToken ct = default);
}
