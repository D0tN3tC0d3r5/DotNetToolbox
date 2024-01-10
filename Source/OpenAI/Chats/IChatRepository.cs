namespace DotNetToolbox.OpenAI.Chats;

internal interface IChatRepository : IDisposable {
    Task<Chat[]> Get();
    Task<Chat?> GetById(string id);
    Task Add(Chat chat);
    Task Delete(string id);
}
