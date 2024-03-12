namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    IAgentOptions Options { get; }
    Task<HttpResult> HandleRequest(IConsumer source, IChat chat, CancellationToken token);
}
