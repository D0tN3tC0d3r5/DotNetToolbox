namespace DotNetToolbox.AI.Chats;

public class Chat(IGuidProvider guid)
    : IChat {

    public Chat()
        : this(new GuidProvider()) {
    }

    public Guid Id { get; set; } = guid.New();
    public Instructions Instructions { get; set; } = new();
    public List<Message> Messages { get; set; } = [];
    public uint TotalTokens { get; set; }
}
