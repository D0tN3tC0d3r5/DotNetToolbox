namespace DotNetToolbox.AI.Agents;

public interface IStandardAgent {
    World World { get; }
    IAgentOptions Options { get; }
    Persona Persona { get; }
    Task<HttpResult> HandleRequest(IConsumer source, IChat chat, CancellationToken ct = default);
}

public interface IStandardAgent<out TOptions> : IStandardAgent
    where TOptions : class, IAgentOptions, new() {
    IAgentOptions IStandardAgent.Options => Options;
    new TOptions Options { get; }
}
