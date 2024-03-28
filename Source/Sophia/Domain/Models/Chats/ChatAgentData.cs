namespace Sophia.Models.Chats;

public class ChatAgentData :
    IHasMessages {
    public int Number { get; set; }
    [Required]
    public string Provider { get; set; } = default!;
    [Required]
    public PersonaData Persona { get; set; } = default!;
    [Required]
    public AgentOptions Options { get; set; } = default!;
    public List<MessageData> Messages { get; set; } = [];
}
