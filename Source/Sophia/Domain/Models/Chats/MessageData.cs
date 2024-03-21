namespace Sophia.Models.Chats;

public class MessageData {
    public required string Content { get; set; }
    public bool IsUserMessage { get; set; }
}
