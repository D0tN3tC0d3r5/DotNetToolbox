namespace DotNetToolbox.AI.Chats;

public class Chat(string id, Instructions instructions, IEnumerable<Message>? messages = null, uint totalTokens = 0)
    : IChat {
    public string Id { get; set; } = id;
    public Instructions Instructions { get; set; } = instructions;
    public List<Message> Messages { get; set; } = messages?.ToList() ?? [];
    public uint TotalTokens { get; set; } = totalTokens;
}
