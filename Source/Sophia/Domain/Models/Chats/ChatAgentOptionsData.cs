namespace Sophia.Models.Chats;

public class ChatAgentOptionsData {
    public int Id;
    [Required]
    public ProviderData Provider { get; set; } = default!;
    [Required]
    public ModelData Model { get; set; } = default!;
    [Range(0, DotNetToolbox.AI.Agents.Model.MaximumRetries)]
    public byte NumberOfRetries { get; set; } = DotNetToolbox.AI.Agents.Model.DefaultNumberOfRetries;
    public uint MaximumOutputTokens { get; set; } = DotNetToolbox.AI.Agents.Model.DefaultMaximumOutputTokens;
    [Range(0, DotNetToolbox.AI.Agents.Model.MaximumTemperature)]
    public decimal Temperature { get; set; } = DotNetToolbox.AI.Agents.Model.DefaultTemperature;
    [Range(0, DotNetToolbox.AI.Agents.Model.MaximumTokenProbabilityCutOff)]
    public decimal TokenProbabilityCutOff { get; set; }
    public List<string> StopSequences { get; set; } = [];
    public bool IsStreaming { get; set; } = false;
    public bool RespondsAsJson { get; set; } = false;

    public string? ValidateStopSequences()
        => StopSequences.Count > 0 && StopSequences.Any(string.IsNullOrWhiteSpace)
               ? "The list of stop signals cannot be null, empty, or contain only whitespace."
               : null;

    public Model ToModel() => new() {
        NumberOfRetries = NumberOfRetries,
        MaximumOutputTokens = MaximumOutputTokens,
        Temperature = Temperature,
        TokenProbabilityCutOff = TokenProbabilityCutOff,
        StopSequences = [.. StopSequences],
        ResponseIsStream = IsStreaming,
        RespondsAsJson = RespondsAsJson,
    };
}
