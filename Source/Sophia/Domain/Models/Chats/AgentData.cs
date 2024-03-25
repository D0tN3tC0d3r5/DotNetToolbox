namespace Sophia.Models.Chats;

public class AgentData {
    [Required]
    public PersonaData Persona { get; set; } = new();
    [Required]
    public string Model { get; set; } = string.Empty;
    [Required]
    public AgentOptions Options { get; set; } = new();
    [Range(0, 2)]
    public double Temperature { get; set; } = 1;
}
