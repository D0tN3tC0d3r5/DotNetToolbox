namespace DotNetToolbox.AI.Chats;

public class Chat(IGuidProvider guid)
    : IChat {
    public Chat()
        : this(new GuidProvider()) {
    }

    public Guid Id { get; set; } = guid.Create();
    public List<Message> Messages { get; set; } = [];
    public uint TotalTokens { get; set; }
}
