namespace DotNetToolbox.AI.Chats;

internal interface IChatRepository<TChat>
    : IDisposable
    where TChat : IChat {
    Task<TChat[]> Get(CancellationToken ct = default);
    Task<TChat?> GetById(string id, CancellationToken ct = default);
    Task Add(TChat chat, CancellationToken ct = default);
    Task Update(string id, TChat chat, CancellationToken ct = default);
    Task Delete(string id, CancellationToken ct = default);
}
