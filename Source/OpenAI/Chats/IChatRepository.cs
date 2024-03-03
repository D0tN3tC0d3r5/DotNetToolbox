namespace DotNetToolbox.OpenAI.Chats;

internal interface IChatRepository : IDisposable {
    Task<Chat[]> Get(CancellationToken ct = default);
    Task<Chat?> GetById(string id, CancellationToken ct = default);
    Task Add(Chat chat, CancellationToken ct = default);
    Task Delete(string id, CancellationToken ct = default);
}
