using System.Linq.Expressions;

namespace Sophia.Models.Chats;

public class ChatData :
    IHasChatMessages {
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

    public static Expression<Func<ChatData, bool>> BuildFilter(string? filter)
        => filter switch {
               "ShowArchived" => (_) => true,
               _ => c => c.IsActive == true,
           };
}
