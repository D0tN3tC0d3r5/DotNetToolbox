namespace Sophia.Models.Chats;

public class ChatAgentData :
    IHasMessages {
    public Guid ChatId { get; set; }
    public int AgentNumber { get; set; }
    [Required]
    public PersonaData Persona { get; set; } = default!;
    [Required]
    public ChatAgentOptionsData Options { get; set; } = default!;
    public List<MessageData> Messages { get; set; } = [];
}
