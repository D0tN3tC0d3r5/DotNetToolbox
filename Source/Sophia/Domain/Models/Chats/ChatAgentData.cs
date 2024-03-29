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
    public List<string> Instructions { get; set; } = [];
    public string? ValidateInstructions()
        => Instructions.Any(string.IsNullOrWhiteSpace)
               ? "Instructions cannot contain empty or whitespace strings."
               : Instructions.Count != Instructions.Distinct().Count()
                   ? "Instructions cannot contain duplicated values."
                   : null;
}
