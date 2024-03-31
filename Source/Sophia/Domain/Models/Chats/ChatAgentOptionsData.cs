namespace Sophia.Models.Chats;

public class ChatAgentOptionsData {
    public int Id { get; set; }
    [Range(0, AgentModel.MaximumRetries)]
    public byte NumberOfRetries { get; set; } = AgentModel.DefaultNumberOfRetries;
    public uint MaximumOutputTokens { get; set; } = AgentModel.DefaultMaximumOutputTokens;
    [Range(0, AgentModel.MaximumTemperature)]
    public decimal Temperature { get; set; } = AgentModel.DefaultTemperature;
    [Range(0, AgentModel.MaximumTokenProbabilityCutOff)]
    public decimal TokenProbabilityCutOff { get; set; }
    public List<string> StopSequences { get; set; } = [];
    public bool IsStreaming { get; set; }
    public bool RespondsAsJson { get; set; }

    public string? ValidateStopSequences()
        => StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace)
               ? "The list of stop signals cannot be null, empty, or contain only whitespace."
               : null;
}
