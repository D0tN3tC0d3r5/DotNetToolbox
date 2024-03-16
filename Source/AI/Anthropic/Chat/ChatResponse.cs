namespace DotNetToolbox.AI.Anthropic.Chat;

public class ChatResponse(string id) : IChatResponse {
    [JsonPropertyName("id")]
    public string Id { get; init; } = id;
    [JsonPropertyName("model")]
    public string? Model { get; init; }
    [JsonPropertyName("completion")]
    public required MessageContent[] Completion { get; set; }
    [JsonPropertyName("stop_reason")]
    public required string StopReason { get; init; }
    [JsonPropertyName("stop_sequence")]
    public required string StopSequence { get; init; }
    [JsonPropertyName("usage")]
    public required Usage Usage { get; init; }
}
