namespace Sophia.Models.Chats;

public class AgentData {
    public PersonaData Persona { get; set; } = new();
    public string Model { get; set; } = string.Empty;
    public double Temperature { get; set; }

    public string? Validate() {
        return string.IsNullOrWhiteSpace(Model)
            ? "Model is required"
            : Temperature is < 0 or > 1 ? "Temperature must be between 0 and 1" : null;
    }
}
