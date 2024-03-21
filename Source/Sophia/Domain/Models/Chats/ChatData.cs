namespace Sophia.Models.Chats;

public class ChatData {
    public int Id { get; set; }
    public string Title { get; set; } = "New Chat";
    public bool IsActive { get; set; } = true;
    public AgentData Agent { get; set; } = new();
    public List<MessageData> Messages { get; set; } = [];

    public string? Validate() {
        return string.IsNullOrWhiteSpace(Title) ? "Title is required" : null;
    }
}
