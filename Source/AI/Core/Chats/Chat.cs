namespace DotNetToolbox.AI.Chats;

public class Chat(string id)
    : IChat {
    public Chat(IStringGuidProvider guid)
        : this(guid.CreateSortable()) {
    }
    public Chat()
        : this(StringGuidProvider.Default) {
    }

    public string Id { get; } = IsNotNull(id);
    public List<Message> Messages { get; } = [];
    public uint InputTokens { get; set; }
    public uint OutputTokens { get; set; }
}
