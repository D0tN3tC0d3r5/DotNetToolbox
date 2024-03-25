namespace Sophia.Models.Chats;

public class ChatData {
    public string Id { get; set; } = default!;
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = "New Chat";
    public bool IsActive { get; set; } = true;
    public ProviderData Provider { get; set; } = new();
    public AgentData Agent { get; set; } = new();
    public List<MessageData> Messages { get; set; } = [];

    public Chat ToModel() {
        var messages = Messages.ToList(i => i.ToModel());
        return new($"{Id}", new(), messages);
    }
}
