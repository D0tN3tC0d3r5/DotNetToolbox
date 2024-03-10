using DotNetToolbox.AI.Agents;

namespace DotNetToolbox.AI.Chats;

public interface IChatHandler {
    Task<HttpResult> Submit(IAgent agent, CancellationToken ct = default);
}
