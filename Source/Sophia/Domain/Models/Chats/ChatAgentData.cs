namespace Sophia.Models.Chats;

public class ChatAgentData :
    IHasChatAgentMessages {
    public Guid ChatId { get; set; }
    public int Number { get; set; }
    [Required]
    public PersonaData Persona { get; set; } = default!;
    [Required]
    [MaxLength(50)]
    public string ModelId { get; set; } = default!;
    public ModelData Model { get; set; } = default!;
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

    public AgentModel ToModel() => new() {
        ModelId = Model.Id,
        NumberOfRetries = Options.NumberOfRetries,
        MaximumOutputTokens = Options.MaximumOutputTokens,
        Temperature = Options.Temperature,
        TokenProbabilityCutOff = Options.TokenProbabilityCutOff,
        StopSequences = [.. Options.StopSequences],
        ResponseIsStream = Options.IsStreaming,
        RespondsAsJson = Options.RespondsAsJson,
    };
}
