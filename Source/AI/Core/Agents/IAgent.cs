namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentModel Model { get; set; }
    World World { get; set; }
    UserProfile UserProfile { get; set; }
    Persona Persona { get; set; }
    Task<HttpResult> SendRequest(IChat chat, CancellationToken ct = default);
}
