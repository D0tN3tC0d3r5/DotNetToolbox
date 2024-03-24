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
    uint MaximumOutputTokens { get; }
    decimal Temperature { get; }
    decimal TokenProbabilityCutOff { get; }
    HashSet<string> StopSequences { get; }
    bool UseStreaming { get; }
    bool JsonMode { get; }
}
