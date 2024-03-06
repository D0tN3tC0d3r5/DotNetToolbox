namespace DotNetToolbox.AI.Repositories;

internal sealed class InMemoryChatRepository<TChat>(ILogger<InMemoryChatRepository<TChat>>? logger = null)
    : IChatRepository<TChat>
    where TChat : IChat {
    private readonly ILogger<InMemoryChatRepository<TChat>> _logger = logger ?? NullLogger<InMemoryChatRepository<TChat>>.Instance;
    private readonly ConcurrentDictionary<string, TChat> _chats = new();

    public void Dispose()
        => _chats.Clear();

    public Task<TChat[]> Get(CancellationToken ct = default) {
        _logger.LogTrace("Getting all chats...");
        return Task.FromResult(_chats.Values.ToArray());
    }

    public Task<TChat?> GetById(string id, CancellationToken ct = default) {
        _logger.LogTrace("Getting chat '{id}'...", id);
        return Task.FromResult(_chats.GetValueOrDefault(id));
    }

    public Task Add(TChat chat, CancellationToken ct = default) {
        _logger.LogTrace("Adding chat '{id}'...", chat.Id);
        _chats.TryAdd(chat.Id, chat);
        return Task.CompletedTask;
    }

    public async Task Update(string id, TChat chat, CancellationToken ct = default) {
        _logger.LogTrace("Adding chat '{id}'...", chat.Id);
        var original = await GetById(id, ct);
        if (original != null) _chats.TryUpdate(id, chat, original);
    }

    public Task Delete(string id, CancellationToken ct = default) {
        _logger.LogTrace("Removing chat '{id}'...", id);
        _chats.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
