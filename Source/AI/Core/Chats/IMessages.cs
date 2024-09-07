namespace DotNetToolbox.AI.Chats;

public interface IMessages : IList<Message> {
    string Id { get; }
    uint InputTokens { get; set; }
    uint OutputTokens { get; set; }

    void SetSystemMessage(string content);
    void AppendUserMessage(string content);
    void AppendAssistantMessage(string content);
}

