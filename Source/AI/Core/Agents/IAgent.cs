namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    AgentModel AgentModel { get; set; }
    World World { get; set; }
    User User { get; set; }
    Persona Persona { get; set; }
    Task<HttpResult> SendRequest(IResponseAwaiter source, IChat chat, int? number, CancellationToken ct = default);
}
