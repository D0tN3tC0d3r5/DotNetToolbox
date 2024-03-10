using DotNetToolbox.AI.Agents;

namespace DotNetToolbox.AI.Chats;

public interface IChatHandler {
    IChat Chat { get; }
    Task<HttpResult> Submit(IAgent agent, CancellationToken ct = default);
}
