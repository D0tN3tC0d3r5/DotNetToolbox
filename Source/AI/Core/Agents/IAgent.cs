namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    World World { get; set; }
    AgentOptions Options { get; set; }
    Persona Persona { get; set; }
    Task<HttpResult> SendRequest(IResponseAwaiter source, IChat chat, int? number, CancellationToken ct = default);
}
