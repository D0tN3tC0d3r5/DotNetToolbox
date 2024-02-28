namespace DotNetToolbox.OpenAI.Repositories;

internal sealed class InMemoryChatRepository(ILogger<InMemoryChatRepository>? logger = null)
    : IAgentRepository {
    private readonly ILogger<InMemoryChatRepository> _logger = logger ?? NullLogger<InMemoryChatRepository>.Instance;
    private readonly ConcurrentDictionary<string, Agent> _chats = new();

    public void Dispose()
        => _chats.Clear();

    public Task<Agent[]> Get(CancellationToken ct = default) {
        _logger.LogTrace("Getting all chats...");
        return Task.FromResult(_chats.Values.ToArray());
    }

    public Task<Agent?> GetById(string id, CancellationToken ct = default) {
        _logger.LogTrace("Getting chat '{id}'...", id);
        return Task.FromResult(_chats.GetValueOrDefault(id));
    }

    public Task Add(Agent chat, CancellationToken ct = default) {
        _logger.LogTrace("Adding chat '{id}'...", chat.Id);
        _chats.TryAdd(chat.Id, chat);
        return Task.CompletedTask;
    }

    public Task Delete(string id, CancellationToken ct = default) {
        _logger.LogTrace("Removing chat '{id}'...", id);
        _chats.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
