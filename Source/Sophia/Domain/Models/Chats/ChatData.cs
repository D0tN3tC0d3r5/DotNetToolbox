namespace Sophia.Models.Chats;

public class ChatData :
    IHasMessages {
    public Guid Id { get; set; } = default!;
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = "New Chat";
    public bool IsActive { get; set; } = true;
    public List<ChatAgentData> Agents { get; set; } = [];
    public List<MessageData> Messages { get; set; } = [];

    public Chat ToModel() {
        var messages = Messages.ToList(i => i.ToModel());
        return new(Id, new(), messages);
    }
}
