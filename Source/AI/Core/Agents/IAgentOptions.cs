namespace DotNetToolbox.AI.Agents;

public interface IAgentOptions : IValidatable {
    public static readonly JsonSerializerOptions SerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };

    string ApiEndpoint { get; }
    string Model { get; }
    byte NumberOfRetries { get; }
    uint MaximumOutputTokens { get; set; }
    decimal? Temperature { get; set; }
    decimal? TokenProbabilityCutOff { get; set; }
    HashSet<string> StopSequences { get; set; }
    bool UseStreaming { get; set; }
}
