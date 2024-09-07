namespace DotNetToolbox.AI.Chats;

public class Messages(string id, IEnumerable<Message>? messages = null)
        : List<Message>(messages ?? []),
      IMessages {
    public Messages(IStringGuidProvider guid, IEnumerable<Message>? messages = null)
        : this(guid.CreateSortable(), messages) {
    }
    public Messages(IEnumerable<Message>? messages = null)
        : this(StringGuidProvider.Default, messages) {
    }

    public string Id { get; } = IsNotNull(id);
    public uint InputTokens { get; set; }
    public uint OutputTokens { get; set; }
}
