namespace DotNetToolbox.AI.Chats;

public class Chat(
        string id,
        Instructions instructions,
        IEnumerable<Message> messages,
        uint totalTokens = default)
    : IChat {

    public Chat(string id)
        : this(id, new(), []) {
    }

    public Chat(ISystemEnvironment environment)
        : this(environment.Guid.New().ToString(), new(), []) {
    }

    public string Id { get; set; } = id;
    public Instructions Instructions { get; set; } = instructions;
    public List<Message> Messages { get; set; } = messages?.ToList() ?? [];
    public uint TotalTokens { get; set; } = totalTokens;
}
