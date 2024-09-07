using Task = DotNetToolbox.AI.Jobs.Task;

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

    public void SetSystemMessage(string content) {
        if (Count == 0) Add(new Message(MessageRole.System, content));
        else this[0] = new Message(MessageRole.System, content);
    }

    public void AppendUserMessage(string content) {
        if (Count == 0) Add(new Message(MessageRole.System, Task.Default.Prompt));
        if (this[^1].Role == MessageRole.User) this[^1].Parts.Add(content);
        else Add(new Message(MessageRole.User, content));
    }

    public void AppendAssistantMessage(string content) {
        if (Count == 0) Add(new Message(MessageRole.System, Task.Default.Prompt));
        if (this[^1].Role == MessageRole.Assistant) this[^1].Parts.Add(content);
        else Add(new Message(MessageRole.Assistant, content));
    }

    public string Id { get; } = IsNotNull(id);
    public uint InputTokens { get; set; }
    public uint OutputTokens { get; set; }
}
