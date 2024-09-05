namespace DotNetToolbox.AI.Agents;

public interface IHttpConnectionSettings : IValidatable {
    public static readonly JsonSerializerOptions SerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };
    byte MaximumNumberOfRetries { get; }
    uint MaximumOutputTokens { get; }
    decimal Temperature { get; }
    decimal TokenProbabilityCutOff { get; }
    List<string> StopSequences { get; }
    bool ResponseIsStream { get; }
    bool ResponseIsJson { get; }
}
