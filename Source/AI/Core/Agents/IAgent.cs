namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    World World { get; }
    IAgentOptions Options { get; }
    Persona Persona { get; }
    Task<HttpResult> SendRequest(IResponseAwaiter source, IChat chat, CancellationToken ct = default);
}
