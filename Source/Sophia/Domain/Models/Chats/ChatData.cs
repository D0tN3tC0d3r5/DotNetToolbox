namespace Sophia.Models.Chats;

public class ChatData {
    public int Id { get; set; }
    public string Title { get; set; } = "New Chat";
    public string Persona { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public double Temperature { get; set; }

    public string? Validate() {
        if (string.IsNullOrWhiteSpace(Persona))
            return "Persona is required";
        if (string.IsNullOrWhiteSpace(Model))
            return "Model is required";
        if (Temperature is < 0 or > 1)
            return "Temperature must be between 0 and 1";
        return null;
    }
}
