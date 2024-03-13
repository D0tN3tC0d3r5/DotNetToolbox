namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    World World { get; }
    IAgentOptions Options { get; }
    IPersona Persona { get; }
    Task<HttpResult> HandleRequest(IConsumer source, IChat chat, CancellationToken ct);
}

public interface IAgent<out TOptions> : IAgent
    where TOptions : class, IAgentOptions, new() {
    IAgentOptions IAgent.Options => Options;
    new TOptions Options { get; }
}
