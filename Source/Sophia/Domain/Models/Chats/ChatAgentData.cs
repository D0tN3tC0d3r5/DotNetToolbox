namespace Sophia.Models.Chats;

public class ChatAgentData :
    IHasMessages {
    [Required]
    public ChatData Chat { get; set; } = new();
    public int Number { get; set; }
    [Required]
    public ProviderData Provider { get; set; } = new();
    [Required]
    public PersonaData Persona { get; set; } = new();
    [Required]
    public AgentOptions Options { get; set; } = new();
    public List<MessageData> Messages { get; set; } = [];
}
