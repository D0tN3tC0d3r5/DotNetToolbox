namespace DotNetToolbox.AI.Chats;

public interface IChat
    : IList<Message> {
    string Id { get; }
    uint CallCount { get; set; }
    uint InputTokens { get; set; }
    uint OutputTokens { get; set; }

    void SetSystemMessage(string prompt);
    void AppendMessage(MessageRole role, string content);
    void AppendMessage(MessageRole role, MessagePart content);
    void AppendMessage(MessageRole role, IEnumerable<MessagePart> content);
}
