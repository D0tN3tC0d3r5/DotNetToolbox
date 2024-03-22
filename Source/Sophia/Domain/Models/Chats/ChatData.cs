namespace Sophia.Models.Chats;

public class ChatData {
    public int Id { get; set; }
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = "New Chat";
    public bool IsActive { get; set; } = true;
    public AgentData Agent { get; set; } = new();
    public List<MessageData> Messages { get; set; } = [];
}
