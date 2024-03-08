namespace DotNetToolbox.AI.Chats;

public interface IChatOptions : IValidatable {
    public static readonly JsonSerializerOptions SerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };

    string SystemMessage { get; set; }
    string Model { get; }
    uint MaximumTokensPerMessage { get; }
    decimal? Temperature { get; }
    decimal? MinimumTokenProbability { get; }
    bool UseStreaming { get; }
    HashSet<string> StopSequences { get; }
}
