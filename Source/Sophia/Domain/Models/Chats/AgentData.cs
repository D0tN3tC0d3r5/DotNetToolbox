namespace Sophia.Models.Chats;

public class AgentData {
    [Required]
    public PersonaData Persona { get; set; } = new();

    [Required]
    public ProviderData Provider { get; set; } = new();

    [Required]
    public AgentOptions Options { get; set; } = new();
}
