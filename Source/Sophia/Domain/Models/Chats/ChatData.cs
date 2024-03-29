namespace Sophia.Models.Chats;

public class ChatData :
    IHasMessages {
    public Guid Id { get; set; } = default!;
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = "New Chat";
    public bool IsActive { get; set; } = true;
    public List<ChatAgentData> Agents { get; set; } = [];
    public InstructionsData Instructions { get; set; } = new();
    public List<MessageData> Messages { get; set; } = [];

    public Chat ToModel()
        => new() {
            Id = Id,
            Instructions = Instructions.ToModel(),
            Messages = Messages.ToList(i => i.ToModel()),
        };
}
