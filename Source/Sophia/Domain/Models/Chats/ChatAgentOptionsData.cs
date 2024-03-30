namespace Sophia.Models.Chats;

public class ChatAgentOptionsData {
    public int Id;
    [Required]
    public ModelData Model { get; set; } = default!;
    [Range(0, AgentModel.MaximumRetries)]
    public byte NumberOfRetries { get; set; } = AgentModel.DefaultNumberOfRetries;
    public uint MaximumOutputTokens { get; set; } = AgentModel.DefaultMaximumOutputTokens;
    [Range(0, AgentModel.MaximumTemperature)]
    public decimal Temperature { get; set; } = AgentModel.DefaultTemperature;
    [Range(0, AgentModel.MaximumTokenProbabilityCutOff)]
    public decimal TokenProbabilityCutOff { get; set; }
    public List<string> StopSequences { get; set; } = [];
    public bool IsStreaming { get; set; } = false;
    public bool RespondsAsJson { get; set; } = false;

    public string? ValidateStopSequences()
        => StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace)
               ? "The list of stop signals cannot be null, empty, or contain only whitespace."
               : null;

    public AgentModel ToModel() => new() {
        ModelId = Model.Id,
        NumberOfRetries = NumberOfRetries,
        MaximumOutputTokens = MaximumOutputTokens,
        Temperature = Temperature,
        TokenProbabilityCutOff = TokenProbabilityCutOff,
        StopSequences = [.. StopSequences],
        ResponseIsStream = IsStreaming,
        RespondsAsJson = RespondsAsJson,
    };
}
